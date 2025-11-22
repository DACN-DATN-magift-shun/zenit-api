using System.Text.Json;

using Dapper;

using Microsoft.Extensions.DependencyInjection;

using Npgsql;

using Zenit.Share.Common.Constants;
using Zenit.Share.Common.Services;
using Zenit.Statistics.Business.Helpers;
using Zenit.Statistics.Contract.Requests;
using Zenit.Statistics.Data;

namespace Zenit.Statistics.Business.Services
{
    public class StatisticsService(IServiceProvider serviceProvider) : StatisticsApplicationService(serviceProvider)
    {
        public DapperQueryService DapperQueryService => ServiceProvider.GetService<DapperQueryService>();
        public StatisticsRedisCache RedisCache => ServiceProvider.GetService<StatisticsRedisCache>();

        public async Task<StatisticsGetAllResponse> GetAll(StatisticsGetAllRequest request)
        {
            DateTime fromDate = request.From ?? new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            DateTime toDate = request.To ?? DateTime.UtcNow;

            int intervalByDays = (toDate - fromDate).Days + 1;

            DateTime previousFromDate = DateTime.UtcNow;
            DateTime previousToDate = DateTime.UtcNow;

            if (intervalByDays > 7)
            {
                previousFromDate = new DateTime(fromDate.AddMonths(-1).Year, fromDate.AddMonths(-1).Month, 1);
                previousToDate = new DateTime(fromDate.Year, fromDate.Month, 1).AddDays(-1);
            }
            else
            {
                previousFromDate = fromDate.AddDays(-intervalByDays);
                previousToDate = toDate.AddDays(-intervalByDays);
            }

            string sql = SqlHelper.GetAllStatisticsBySpecificInterval();

            var parameters = new
            {
                FromDate = fromDate,
                ToDate = toDate,
                PreviousFromDate = previousFromDate,
                PreviousToDate = previousToDate,
                AccountId = CurrentAccount.Id
            };

            var cacheKey = $"{sql}{JsonSerializer.Serialize(parameters)}";
            var md5CacheKey = CacheHelper.GetMD5Hash(cacheKey);

            var cachedData = await RedisCache.GetAsync<StatisticsGetAllResponse>(md5CacheKey);
            if (cachedData != null)
            {
                return cachedData;
            }

            var sqlResult = DapperQueryService.QuerySingle<string>(sql, parameters);

            Console.WriteLine("✅ SQL Result: " + sqlResult);

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var result = new StatisticsGetAllResponse
            {
                Items = JsonSerializer.Deserialize<IEnumerable<StatisticsResponseItem>>(sqlResult, jsonOptions) ?? []
            };

            await RedisCache.AddAsync(md5CacheKey, result, DateTimeOffset.UtcNow.AddMinutes(5));

            var first = result.Items.FirstOrDefault();
            if (first != null)
            {
                Console.WriteLine($"First TotalAmount={first.TotalAmount}, Percentage={first.Percentage}, GroupType={first.GroupType}");
                Console.WriteLine("Categories: " + JsonSerializer.Serialize(first.Categories, new JsonSerializerOptions { WriteIndented = true }));
            }

            return Mapper.Map<StatisticsGetAllResponse>(result); 
        }
    }
}
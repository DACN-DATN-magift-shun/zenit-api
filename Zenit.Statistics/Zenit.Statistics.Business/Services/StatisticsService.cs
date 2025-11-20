using System.Text.Json;

using Microsoft.Extensions.DependencyInjection;

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

            string sql = @"
                WITH CurrentIntervalTotalValue AS (
                    SELECT 
                        SUM(TotalAmount) AS TotalAmount
                    FROM TransactionStatistics
                    WHERE Date >= @fromDate AND Date <= @toDate 
                ), CurrentIntervalGroupTypeValue AS (
                    SELECT 
                        GroupType,
                        NULL AS CategoryId,
                        SUM(TotalAmount) AS TotalAmount
                    FROM TransactionStatistics
                    WHERE Date >= @fromDate AND Date <= @toDate 
                    GROUP BY GroupType
                ), PreviousIntervalGroupTypeValue AS (
                    SELECT 
                        GroupType,
                        NULL AS CategoryId,
                        SUM(TotalAmount) AS TotalAmount
                    FROM TransactionStatistics
                    WHERE Date >= @previousFromDate AND Date <= @previousToDate 
                    GROUP BY GroupType
                ), CurrentIntervalCategoryValue AS (
                    SELECT
                        GroupType,
                        CategoryId,
                        SUM(TotalAmount) AS TotalAmount
                    FROM TransactionStatistics
                    WHERE Date >= @fromDate AND Date <= @toDate 
                    GROUP BY GroupType, CategoryId
                ), PreviousIntervalCategoryValue AS (
                    SELECT
                        GroupType,
                        CategoryId,
                        SUM(TotalAmount) AS TotalAmount
                    FROM TransactionStatistics
                    WHERE Date >= @previousFromDate AND Date <= @previousToDate 
                    GROUP BY GroupType, CategoryId
                ), GroupTypeStatistics AS (
                    SELECT
                        c.GroupType,
                        c.TotalAmount,
                        ROUND((c.TotalAmount / t.TotalAmount) * 100, 2) AS Percentage,
                        ROUND(
                            CASE 
                                WHEN p.TotalAmount IS NULL OR p.TotalAmount = 0 THEN 100.0
                                ELSE ((c.TotalAmount - p.TotalAmount) / p.TotalAmount) * 100
                            END, 2
                        ) AS PercentageChange
                    FROM CurrentIntervalGroupTypeValue c
                    CROSS JOIN CurrentIntervalTotalValue t
                    LEFT JOIN PreviousIntervalGroupTypeValue p
                    ON c.GroupType = p.GroupType
                ), CategoryStatistics AS (
                    SELECT
                        c.GroupType,
                        c.CategoryId,
                        c.TotalAmount,
                        ROUND((c.TotalAmount / gt.TotalAmount) * 100, 2) AS Percentage,
                        ROUND(
                            CASE 
                                WHEN p.TotalAmount IS NULL OR p.TotalAmount = 0 THEN 100.0
                                ELSE ((c.TotalAmount - p.TotalAmount) / p.TotalAmount) * 100
                            END, 2
                        ) AS PercentageChange
                    FROM CurrentIntervalCategoryValue c
                    LEFT JOIN PreviousIntervalCategoryValue p
                        ON c.GroupType = p.GroupType AND c.CategoryId = p.CategoryId
                    INNER JOIN CurrentIntervalGroupTypeValue gt
                        ON c.GroupType = gt.GroupType
                ) 
                SELECT json_agg (
                    json_build_object (
                        'TotalAmount', gt.TotalAmount,
                        'Percentage', gt.Percentage,
                        'PercentageChange', gt.PercentageChange,
                        'GroupType', gt.GroupType,
                        'Details', (
                            SELECT json_agg (
                                json_build_object (
                                    'CategoryId', cs.CategoryId,
                                    'TotalAmount', cs.TotalAmount,
                                    'Percentage', cs.Percentage,
                                    'PercentageChange', cs.PercentageChange
                                )
                            )
                            FROM CategoryStatistics cs
                            WHERE cs.GroupType = gt.GroupType
                        )
                    )
                ) as Items
                FROM GroupTypeStatistics gt;
            ";

            var parameters = new
            {
                fromDate,
                toDate,
                previousFromDate,
                previousToDate
            };

            var cacheKey = $"{sql}{JsonSerializer.Serialize(parameters)}";
            var md5CacheKey = CacheHelper.GetMD5Hash(cacheKey);

            var cachedData = await RedisCache.GetAsync<StatisticsGetAllResponse>(md5CacheKey);
            if (cachedData != null)
            {
                return cachedData;
            }

            var sqlResult = DapperQueryService.QuerySingle<string>(sql, parameters);

            var result = JsonSerializer.Deserialize<StatisticsGetAllResponse>(sqlResult);

            await RedisCache.AddAsync(md5CacheKey, result, DateTimeOffset.UtcNow.AddMinutes(5));

            return result;
        }
    }
}
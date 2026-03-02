using System.Text.Json;

using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;

using Zenit.Management.Business.Helpers;
using Zenit.Management.Business.Services.CategoryServices;
using Zenit.Management.Contract.Requests.CategoryRequests;
using Zenit.Management.Contract.Requests.StatisticsRequests;
using Zenit.Management.Data;
using Zenit.Management.Data.Entities;
using Zenit.Share.Common.Constants;
using Zenit.Share.Common.Values;

namespace Zenit.Management.Business.Services
{
    public class StatisticsService(IServiceProvider serviceProvider) : ManagementApplicationService(serviceProvider)
    {
        public ManagementDapperQueryService DapperQueryService => ServiceProvider.GetService<ManagementDapperQueryService>();
        public ManagementRedisCache RedisCache => ServiceProvider.GetService<ManagementRedisCache>();

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

            var jsonDocument = JsonDocument.Parse(sqlResult);
            var root = jsonDocument.RootElement;

            var result = new StatisticsGetAllResponse
            {
                GroupStatistics = JsonSerializer.Deserialize<IEnumerable<StatisticsResponseItem>>(root.GetProperty("GroupStatistics").GetRawText(), jsonOptions) ?? [],
                IncomeExpenseStatistics = JsonSerializer.Deserialize<IncomeExpenseStatistics>(root.GetProperty("IncomeExpenseSummary").GetRawText(), jsonOptions)
            };

            await RedisCache.AddAsync(md5CacheKey, result, DateTimeOffset.UtcNow.AddMinutes(5));

            return Mapper.Map<StatisticsGetAllResponse>(result);
        }

        // TODO: Implement handle transaction event handlers here
        public async Task HandleCreateTransactionAsync(Transaction transaction, List<AuditDataChange> dataChanges)
        {
            // Implementation for handling transaction created event
            string sql = SqlHelper.UpsertDailyStatisticsWithAddedTransactions();

            var parameters = new
            {
                Date = transaction.TransactionDate,
                TotalAmount = transaction.Amount,
                CategoryId = transaction.CategoryId,
                AccountId = transaction.AccountId,
                GroupType = transaction.Category!.GroupType,
                CategoryStatsId = Guid.NewGuid(),
                CategoryGroupStatsId = Guid.NewGuid(),
                CreatedById = transaction.AccountId,
                ModifiedById = transaction.AccountId
            };
            try
            {
                DapperQueryService.Execute(sql, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error executing SQL: " + ex.Message);
                throw;
            }
        }

        public async Task HandleUpdateTransactionAsync(Transaction transaction, List<AuditDataChange> dataChanges)
        {
            // Implementation for handling transaction created event
            string sql = SqlHelper.UpsertDailyStatisticsWithModifiedTransactions();

            var oldCategoryId = dataChanges.FirstOrDefault(dc => dc.Field == nameof(Transaction.CategoryId))?.OriginalValue;
            var categoryService = ServiceProvider.GetService<CategoryService>();
            var oldGroupType = categoryService.CategoryGetDetail(new GetCategoryRequest
            {
                Id = Guid.Parse(oldCategoryId.ToString())
            }).Result.GroupType;

            var parameters = new
            {
                Date = transaction.TransactionDate,
                TotalAmount = transaction.Amount,
                CategoryId = transaction.CategoryId,
                AccountId = transaction.AccountId,
                GroupType = transaction.Category!.GroupType,
                ModifiedById = transaction.ModifiedById,
                OldAmount = dataChanges.FirstOrDefault(dc => dc.Field == nameof(Transaction.Amount))?.OriginalValue,
                OldTransactionDate = dataChanges.FirstOrDefault(dc => dc.Field == nameof(Transaction.TransactionDate))?.OriginalValue,
                OldCategoryId = oldCategoryId,
                OldGroupType = oldGroupType,
                CategoryStatsId = Guid.NewGuid(),
                CategoryGroupStatsId = Guid.NewGuid(),
                CreatedById = transaction.AccountId
            };

            try
            {
                DapperQueryService.Execute(sql, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error executing SQL: " + ex.Message);
                throw;
            }
        }

        public async Task HandleDeleteTransactionAsync(Transaction transaction, List<AuditDataChange> dataChanges)
        {
            // Implementation for handling transaction created event
            string sql = SqlHelper.UpsertDailyStatisticsWithDeletedTransactions();

            var parameters = new
            {
                Date = transaction.TransactionDate,
                TotalAmount = transaction.Amount,
                CategoryId = transaction.CategoryId,
                AccountId = transaction.AccountId,
                GroupType = transaction.Category!.GroupType,
                ModifiedById = transaction.AccountId
            };

            try
            {
                DapperQueryService.Execute(sql, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error executing SQL: " + ex.Message);
                throw;
            }
        }

        public async Task<ReportCreateResponse> GenerateReport(ReportCreateRequest request)
        {
            DateTime fromDate = request.FromDate;
            DateTime toDate = request.ToDate;

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

            var sqlResult = DapperQueryService.QuerySingle<string>(sql, parameters);

            Console.WriteLine("✅ SQL Result: " + sqlResult);

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var jsonDocument = JsonDocument.Parse(sqlResult);
            var root = jsonDocument.RootElement;

            var result = new StatisticsGetAllResponse
            {
                GroupStatistics = JsonSerializer.Deserialize<IEnumerable<StatisticsResponseItem>>(root.GetProperty("GroupStatistics").GetRawText(), jsonOptions) ?? [],
                IncomeExpenseStatistics = JsonSerializer.Deserialize<IncomeExpenseStatistics>(root.GetProperty("IncomeExpenseSummary").GetRawText(), jsonOptions)
            };

            string pdfReportFilePath =  ReportHelper.GeneratePdfReport(result.GroupStatistics, result.IncomeExpenseStatistics, CurrentAccount.Id);

            var s3Client = new AmazonS3Client(RegionEndpoint.APSoutheast1);

            string s3Bucket = Environment.GetEnvironmentVariable(EnvConstants.AWS_S3_BUCKET);
            string fileName = Path.GetFileName(pdfReportFilePath);

            await s3Client.PutObjectAsync(new PutObjectRequest
            {
                BucketName = s3Bucket,
                Key = $"reports/{fileName}",
                FilePath = pdfReportFilePath,
                ContentType = "application/pdf",
            });

            File.Delete(pdfReportFilePath);

            // Tạo pre-signed URL thay vì public URL
            var preSignedUrl = s3Client.GetPreSignedURL(new GetPreSignedUrlRequest
            {
                BucketName = s3Bucket,
                Key = $"reports/{fileName}",
                Expires = DateTime.UtcNow.AddHours(24) // hết hạn sau 24h
            });

            return new ReportCreateResponse
            {
                ReportUrl = preSignedUrl
            };
        }
    }
}
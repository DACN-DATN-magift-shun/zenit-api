using System.Text;
using System.Text.Json;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Zenit.Share.Common.Services;
using Zenit.Share.Data.Interfaces;
using Zenit.Statistics.Business.Helpers;
using Zenit.Statistics.Data.Entities;
using Zenit.Statistics.Data.Models;

namespace Zenit.Statistics.Business.Workers
{
    public class BackgroundConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BackgroundConsumer> _logger;

        public BackgroundConsumer(IServiceProvider serviceProvider, ILogger<BackgroundConsumer> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public RabbitmqConsumerService RabbitmqConsumerService =>
            _serviceProvider.GetRequiredService<RabbitmqConsumerService>();

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // ✅ Đăng ký consumer CHỈ 1 LẦN - không cần while loop
            var consumerTasks = new[]
            {
                Task.Run(async () =>
                {
                    try
                    {
                        _logger.LogInformation("Starting transaction.created consumer");

                        await RabbitmqConsumerService.ConsumeMessageAsync(
                            async (sender, ea) =>
                            {
                                try
                                {
                                    byte[] bodyBytes = ea.Body.ToArray();
                                    string bodyString = Encoding.UTF8.GetString(bodyBytes);

                                    _logger.LogInformation($"Received message: {bodyString}");


                                    var transactions = JsonSerializer.Deserialize<List<TransactionModel>>(bodyString);

                                    var scope = _serviceProvider.CreateScope();
                                    var dapperQuery = scope.ServiceProvider.GetRequiredService<DapperQueryService>();

                                    foreach (var transaction in transactions)
                                    {
                                        string sql = SqlHelper.UpsertDailyStatistics();

                                        var parameters = new
                                        {
                                            Date = transaction.TransactionDate,
                                            TotalAmount = transaction.Amount,
                                            CategoryId = transaction.CategoryId,
                                            AccountId = transaction.AccountId,
                                            GroupType = transaction.GroupType,
                                            CategoryStatsId = Guid.NewGuid(),
                                            CategoryGroupStatsId = Guid.NewGuid(),
                                            CreatedById = transaction.AccountId,
                                            ModifiedById = transaction.AccountId,
                                        };

                                        dapperQuery.Execute(sql, parameters);

                                    }

                                    _logger.LogInformation($"Successfully processed {transactions.Count} transactions");
                                }
                                catch (JsonException jsonEx)
                                {
                                    _logger.LogError(jsonEx, "Failed to deserialize message");
                                    throw; // Để RabbitmqConsumerService xử lý nack
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError(ex, "Error processing transaction.created message");
                                    throw;
                                }
                            },
                            new RabbitmqConsumerRequest
                            {
                                Exchange = "transaction.direct.create",
                                Queue = "transaction.statistics.created", // ✅ Thêm Queue name
                                RoutingKey = "transaction.created",
                                ExchangeType = "direct"
                            }
                        );
                        
                        // ✅ Giữ task alive cho đến khi cancellation
                        await Task.Delay(Timeout.Infinite, stoppingToken);
                    }
                    catch (OperationCanceledException)
                    {
                        _logger.LogInformation("Consumer stopped gracefully");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Fatal error in transaction.created consumer");
                        throw;
                    }
                }, stoppingToken)
            };

            await Task.WhenAll(consumerTasks);
        }
    }
}
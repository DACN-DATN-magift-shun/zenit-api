using System.Text.Json;

using Mapster;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Zenit.Management.Business.Managers;
using Zenit.Management.Contract.TransactionRequests;
using Zenit.Management.Data.Entities;
using Zenit.Management.Data.Models;
using Zenit.Share.Common.Services;
using Zenit.Share.Contract.Models;

namespace Zenit.Management.Business.Services.TransactionService
{
    public class TransactionService(IServiceProvider serviceProvider) : ManagementApplicationService(serviceProvider)
    {
        public TransactionManager _TransactionManager => ServiceProvider.GetService<TransactionManager>();

        // public RabbitmqProducerService _RabbitmqProducerService => ServiceProvider.GetService<RabbitmqProducerService>();

        public Task<GetAllTransactionResponse> GetAll(GetAllTransactionRequest request)
        {
            var transactionsQuery = _TransactionManager.GetAll()
                .Where(t => t.IsDeleted == false && t.AccountId == CurrentAccount.Id);
            
            if (!string.IsNullOrEmpty(request.Search))
            {
                transactionsQuery = transactionsQuery
                    .Where(t => t.Title.Contains(request.Search))
                    .Where(t => t.Note != null && t.Note.Contains(request.Search));
            }

            if (request.FromDate.HasValue && request.ToDate.HasValue)
            {
                transactionsQuery = transactionsQuery
                    .Where(t => t.TransactionDate >= request.FromDate.Value && t.TransactionDate <= request.ToDate.Value);
            }
            
            if (request.CategoryId.HasValue)
            {
                transactionsQuery = transactionsQuery
                    .Where(t => t.CategoryId == request.CategoryId.Value);
            }

            return Task.FromResult(Mapper.Map<GetAllTransactionResponse>(
                PaginationResponse<Transaction>.Create(transactionsQuery, request)
            ));
        }

        public Task<GetDetailTransactionResponse> GetDetail(GetDetailTransactionRequest request)
        {
            var transaction = _TransactionManager.FindBy(t => t.Id == request.Id && t.IsDeleted == false).FirstOrDefault();
            return Task.FromResult(Mapper.Map<GetDetailTransactionResponse>(transaction));
        }

        public async Task<UpdateTransactionResponse> Create(CreateTransactionRequest request)
        {

            var transaction = Mapper.Map<Transaction>(request);
            transaction.Id = Guid.CreateVersion7();
            transaction.AccountId = CurrentAccount.Id;
            _TransactionManager.Add(transaction);

            await UnitOfWork.SaveChangesAsync();

            // var trackedTransaction = _TransactionManager.FindBy(t => t.Id == transaction.Id)
            //                                             .Include(t => t.Category)
            //                                             .FirstOrDefault();

            // var transactionPublishedList = new TransactionPublishedModel
            // {
            //     Amount = transaction.Amount,
            //     TransactionDate = transaction.TransactionDate,
            //     CategoryId = transaction.CategoryId,
            //     AccountId = transaction.AccountId,
            //     GroupType = trackedTransaction.Category.GroupType
            // };

            // try
            // {
            //     await _RabbitmqProducerService.PublishMessageAsync(new RabbitmqProducerRequest
            //     {
            //         Exchange = "transaction.direct.create",
            //         RoutingKey = "transaction.created",
            //         Body = JsonSerializer.Serialize(new[] { transactionPublishedList }),
            //         ExchangeType = "direct"
            //     });
            // }
            // catch (Exception ex)
            // {
            //     // Log the exception (implementation depends on your logging framework)
            //     throw new Exception($"Failed to publish message to RabbitMQ: {ex.Message}");
            // }
            return Mapper.Map<UpdateTransactionResponse>(transaction);
        }

        public async Task<CreateManyTransactionsResponse> CreateMany(CreateManyTransactionsRequest request)
        {
            var transactions = request.Transactions;
            var response = new List<CreateTransactionResponse>();
            var addedTransactions = new List<Transaction>();

            foreach (var transaction in transactions)
            {
                var newTransaction = Mapper.Map<Transaction>(transaction);
                newTransaction.Id = Guid.CreateVersion7();
                newTransaction.AccountId = CurrentAccount.Id;

                addedTransactions.Add(newTransaction);
                response.Add(Mapper.Map<CreateTransactionResponse>(newTransaction));
            }

            _TransactionManager.AddRange(addedTransactions);
            await UnitOfWork.SaveChangesAsync();

            // try {
            //     var transactionPublishedList = addedTransactions.Select(t =>
            //     {
            //         var trackedTransaction = _TransactionManager.FindBy(tr => tr.Id == t.Id)
            //                                                     .Include(tr => tr.Category)
            //                                                     .FirstOrDefault();

            //         return new TransactionPublishedModel
            //         {
            //             Amount = t.Amount,
            //             TransactionDate = t.TransactionDate,
            //             CategoryId = t.CategoryId,
            //             AccountId = t.AccountId,
            //             GroupType = trackedTransaction.Category.GroupType
            //         };
            //     }).ToList();

            //     await _RabbitmqProducerService.PublishMessageAsync(new RabbitmqProducerRequest
            //     {
            //         Exchange = "transaction.direct.create",
            //         RoutingKey = "transaction.created",
            //         Body = JsonSerializer.Serialize(transactionPublishedList),
            //         ExchangeType = "direct"
            //     });
            // } catch (Exception ex)
            // {
            //     throw new Exception($"Failed: {ex.Message}");
            // }

            var result = Mapper.Map<CreateManyTransactionsResponse>(
                new CreateManyTransactionsResponse
                {
                    Transactions = response
                }
           );

            return result;
        }

        public async Task<UpdateTransactionResponse> Update(UpdateTransactionRequest request)
        {
            var transaction = _TransactionManager.FindBy(t => t.Id == request.Id).FirstOrDefault();

            if (transaction == null)
            {
                throw new Exception("Transaction not found");
            }

            var oldTransaction = Mapper.Map<Transaction>(transaction);
            request.Adapt(transaction);

            _TransactionManager.Update(transaction);
            await UnitOfWork.SaveChangesAsync();

            // var trackedTransaction = _TransactionManager.FindBy(t => t.Id == transaction.Id)
            //                                     .Include(t => t.Category)
            //                                     .FirstOrDefault();
            
            // var publishedModel = new TransactionPublishedModel
            // {
            //     Amount = transaction.Amount,
            //     TransactionDate = transaction.TransactionDate,
            //     CategoryId = transaction.CategoryId,
            //     AccountId = transaction.AccountId,
            //     GroupType = trackedTransaction.Category.GroupType,
            //     OldAmount = oldTransaction.Amount,
            //     OldTransactionDate = oldTransaction.TransactionDate,
            //     OldCategoryId = oldTransaction.CategoryId,
            //     OldGroupType = oldTransaction.Category.GroupType
            // };

            // var transactionPublishedList = new List<TransactionPublishedModel> { publishedModel };

            // await _RabbitmqProducerService.PublishMessageAsync(new RabbitmqProducerRequest
            // {
            //     Exchange = "transaction.direct.update",
            //     RoutingKey = "transaction.updated",
            //     Body = JsonSerializer.Serialize(transactionPublishedList),
            //     ExchangeType = "direct"
            // });

            return Mapper.Map<UpdateTransactionResponse>(transaction);
        }

        public async Task<UpdateManyTransactionsResponse> UpdateMany(UpdateManyTransactionsRequest request)
        {
            var requestTransactions = request.Transactions;
            var beforeUpdateTransactions = new List<Transaction>();
            var updatedTransactions = new List<Transaction>();
            var response = new List<UpdateTransactionResponse>();

            foreach (var transaction in requestTransactions)
            {
                var existingTransaction = _TransactionManager.FindBy(t => t.Id == transaction.Id).FirstOrDefault();
                var existingTransactionCategory = _TransactionManager.FindBy(t => t.Id == transaction.Id)
                                                            .Include(t => t.Category)
                                                            .FirstOrDefault();

                // beforeUpdateTransactions.Add(new Transaction
                // {
                //     Id = existingTransaction.Id,
                //     Title = existingTransaction.Title,
                //     Amount = existingTransaction.Amount,
                //     TransactionDate = existingTransaction.TransactionDate,
                //     CategoryId = existingTransaction.CategoryId,
                //     AccountId = existingTransaction.AccountId,
                //     CreatedAt = existingTransaction.CreatedAt,
                //     Category =  existingTransactionCategory.Category,
                // });

                // if (existingTransaction == null)
                // {
                //     throw new Exception($"Transaction with Id {transaction.Id} not found");
                // }

                transaction.Adapt(existingTransaction);
                updatedTransactions.Add(existingTransaction);
            }

            _TransactionManager.UpdateRange(updatedTransactions);
            await UnitOfWork.SaveChangesAsync();

            // var transactionPublishedList = updatedTransactions.Select(t =>
            // {
            //     var trackedTransaction = _TransactionManager.FindBy(tr => tr.Id == t.Id)
            //                                                 .Include(tr => tr.Category)
            //                                                 .FirstOrDefault();
                
            //     var oldTransaction = beforeUpdateTransactions.FirstOrDefault(rt => rt.Id == t.Id);

            //     return new TransactionPublishedModel
            //     {
            //         Amount = t.Amount,
            //         TransactionDate = t.TransactionDate,
            //         CategoryId = t.CategoryId,
            //         AccountId = t.AccountId,
            //         GroupType = trackedTransaction.Category.GroupType,
            //         OldAmount = oldTransaction.Amount,
            //         OldTransactionDate = oldTransaction.TransactionDate,
            //         OldCategoryId = oldTransaction.CategoryId,
            //         OldGroupType = oldTransaction.Category.GroupType
            //     };
            // }).ToList();

            // await _RabbitmqProducerService.PublishMessageAsync(new RabbitmqProducerRequest
            // {
            //     Exchange = "transaction.direct.update",
            //     RoutingKey = "transaction.updated",
            //     Body = JsonSerializer.Serialize(transactionPublishedList),
            //     ExchangeType = "direct"
            // });

            // foreach (var transaction in updatedTransactions)
            // {
            //     response.Add(Mapper.Map<UpdateTransactionResponse>(transaction));
            // }

            return Mapper.Map<UpdateManyTransactionsResponse>(
                new UpdateManyTransactionsResponse
                {
                    Transactions = response
                }
            );
        }

        public async Task Delete(DeleteTransactionRequest request)
        {
            var transaction = _TransactionManager.FindBy(t => t.Id == request.Id).FirstOrDefault();

            if (transaction == null)
            {
                throw new Exception("Transaction not found");
            }

            _TransactionManager.Delete(transaction);
            await UnitOfWork.SaveChangesAsync();

            // var transactionPublishedList = new List<TransactionPublishedModel>
            // {
            //     new TransactionPublishedModel
            //     {
            //         Amount = transaction.Amount,
            //         TransactionDate = transaction.TransactionDate,
            //         CategoryId = transaction.CategoryId,
            //         AccountId = transaction.AccountId,
            //         GroupType = transaction.Category.GroupType
            //     }
            // };

            // await _RabbitmqProducerService.PublishMessageAsync(new RabbitmqProducerRequest
            // {
            //     Exchange = "transaction.direct.delete",
            //     RoutingKey = "transaction.deleted",
            //     Body = JsonSerializer.Serialize(transactionPublishedList),
            //     ExchangeType = "direct"
            // });
        }
    }
}
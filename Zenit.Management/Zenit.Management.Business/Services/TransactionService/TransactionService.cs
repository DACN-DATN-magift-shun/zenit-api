using System.Text.Json;

using Mapster;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Zenit.Management.Business.Managers;
using Zenit.Management.Contract.TransactionRequests;
using Zenit.Management.Data.Entities;
using Zenit.Management.Data.Models;
using Zenit.Share.Common.Services;

namespace Zenit.Management.Business.Services.TransactionService
{
    public class TransactionService(IServiceProvider serviceProvider) : ManagementApplicationService(serviceProvider)
    {
        public TransactionManager _TransactionManager => ServiceProvider.GetService<TransactionManager>();

        public RabbitmqProducerService _RabbitmqProducerService => ServiceProvider.GetService<RabbitmqProducerService>();

        public Task<GetAllTransactionResponse> GetAll(GetAllTransactionRequest request)
        {
            var transactions = _TransactionManager.GetAll().Where(t => t.AccountId == CurrentAccount.Id).ToList();
            return Task.FromResult(Mapper.Map<GetAllTransactionResponse>(transactions));
        }

        public Task<GetDetailTransactionResponse> GetDetail(GetDetailTransactionRequest request)
        {
            var transaction = _TransactionManager.FindBy(t => t.Id == request.Id).FirstOrDefault();
            return Task.FromResult(Mapper.Map<GetDetailTransactionResponse>(transaction));
        }

        public async Task<CreateTransactionResponse> Create(CreateTransactionRequest request)
        {

            var transaction = Mapper.Map<Transaction>(request);
            transaction.Id = Guid.NewGuid();
            transaction.AccountId = CurrentAccount.Id;
            _TransactionManager.Add(transaction);
        
            await UnitOfWork.SaveChangesAsync();

            var trackedTransaction = _TransactionManager.FindBy(t => t.Id == transaction.Id)
                                                        .Include(t => t.Category)
                                                        .FirstOrDefault();

            var transactionPublishedList = new TransactionPublishedModel
            {
                Amount = transaction.Amount,
                TransactionDate = transaction.TransactionDate,
                CategoryId = transaction.CategoryId,
                AccountId = transaction.AccountId,
                GroupType = trackedTransaction.Category.GroupType
            };

            try
            {
                await _RabbitmqProducerService.PublishMessageAsync(new RabbitmqProducerRequest
                {
                    Exchange = "transaction.direct.create",
                    RoutingKey = "transaction.created",
                    Body = JsonSerializer.Serialize(new[] { transactionPublishedList }),
                    ExchangeType = "direct"
                });
            }
            catch (Exception ex)
            {
                // Log the exception (implementation depends on your logging framework)
                throw new Exception($"Failed to publish message to RabbitMQ: {ex.Message}");
            }

            

            return Mapper.Map<CreateTransactionResponse>(transaction);
        }

        public async Task<CreateManyTransactionsResponse> CreateMany(CreateManyTransactionsRequest request)
        {
            var transactions = request.Transactions;
            var response = new List<CreateTransactionResponse>();
            var addedTransactions = new List<Transaction>();

            foreach (var transaction in transactions)
            {
                var newTransaction = Mapper.Map<Transaction>(transaction);
                newTransaction.Id = Guid.NewGuid();
                newTransaction.AccountId = CurrentAccount.Id;

                addedTransactions.Add(newTransaction);
                response.Add(Mapper.Map<CreateTransactionResponse>(newTransaction));
            }

            _TransactionManager.AddRange(addedTransactions);

            await UnitOfWork.SaveChangesAsync();

            await _RabbitmqProducerService.PublishMessageAsync(new RabbitmqProducerRequest
            {
                Exchange = "transaction.direct.create",
                RoutingKey = "transaction.created",
                Body = JsonSerializer.Serialize(Mapper.Map<List<TransactionPublishedModel>>(addedTransactions)),
                ExchangeType = "direct"
            });

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

            var transactionList = new List<(Transaction, Transaction)> { (oldTransaction, transaction) };
            var transactionPublishedList = transactionList.Adapt<List<(TransactionPublishedModel, TransactionPublishedModel)>>();

            await _RabbitmqProducerService.PublishMessageAsync(new RabbitmqProducerRequest
            {
                Exchange = "transaction.direct.update",
                RoutingKey = "transaction.updated",
                Body = JsonSerializer.Serialize(transactionPublishedList),
                ExchangeType = "direct"
            });

            return Mapper.Map<UpdateTransactionResponse>(transaction);
        }

        public async Task<UpdateManyTransactionsResponse> UpdateMany(UpdateManyTransactionsRequest request)
        {
            var requestTransactions = request.Transactions;
            var updatedTransactions = new List<Transaction>();
            var response = new List<CreateTransactionResponse>();

            foreach (var transaction in requestTransactions)
            {
                var existingTransaction = _TransactionManager.FindBy(t => t.Id == transaction.Id).FirstOrDefault();

                if (existingTransaction == null)
                {
                    throw new Exception($"Transaction with Id {transaction.Id} not found");
                }

                transaction.Adapt(existingTransaction);

                updatedTransactions.Add(Mapper.Map<Transaction>(existingTransaction));
                response.Add(Mapper.Map<CreateTransactionResponse>(existingTransaction));
            }

            _TransactionManager.UpdateRange(updatedTransactions);

            await UnitOfWork.SaveChangesAsync();

            var updatedTransactionsPublishedList = Mapper.Map<List<(TransactionPublishedModel, TransactionPublishedModel)>>(
                requestTransactions.Zip(
                    updatedTransactions,
                    (req, updated) => (Mapper.Map<TransactionPublishedModel>(req), Mapper.Map<TransactionPublishedModel>(updated))
                )
            );

            await _RabbitmqProducerService.PublishMessageAsync(new RabbitmqProducerRequest
            {
                Exchange = "transaction.direct.update_many",
                RoutingKey = "transactions.updated",
                Body = JsonSerializer.Serialize(updatedTransactionsPublishedList),
                ExchangeType = "direct"
            });

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

            var transactionList = new List<Transaction> { transaction };
            var transactionPublishedList = transactionList.Adapt<List<TransactionPublishedModel>>();

            await _RabbitmqProducerService.PublishMessageAsync(new RabbitmqProducerRequest
            {
                Exchange = "transaction.direct.delete",
                RoutingKey = "transaction.deleted",
                Body = JsonSerializer.Serialize(transactionPublishedList),
                ExchangeType = "direct"
            });

            await UnitOfWork.SaveChangesAsync();
        }
    }
}
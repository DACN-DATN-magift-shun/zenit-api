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
                transaction.Adapt(existingTransaction);
                updatedTransactions.Add(existingTransaction);
            }

            _TransactionManager.UpdateRange(updatedTransactions);
            await UnitOfWork.SaveChangesAsync();

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
        }
    }
}
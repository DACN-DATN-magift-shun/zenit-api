using Mapster;

using Microsoft.Extensions.DependencyInjection;

using Zenit.Management.Business.Managers;
using Zenit.Management.Contract.TransactionRequests;
using Zenit.Management.Data.Entities;

namespace Zenit.Management.Business.Services.TransactionService
{
    public class TransactionService(IServiceProvider serviceProvider) : ManagementApplicationService(serviceProvider)
    {
        private TransactionManager _TransactionManager => ServiceProvider.GetService<TransactionManager>();

        public Task<GetAllTransactionResponse> GetAll(GetAllTransactionRequest request)
        {
            var transactions = _TransactionManager.GetAll().Where(t => t.UserId == CurrentAccount.Id).ToList();
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
            transaction.UserId = CurrentAccount.Id;
            _TransactionManager.Add(transaction);

            await UnitOfWork.SaveChangesAsync();

            return Mapper.Map<CreateTransactionResponse>(transaction);
        }

        public async Task<CreateManyTransactionsResponse> CreateMany(CreateManyTransactionsRequest request)
        {
            var transactions = request.Transactions;
            var response = new List<TransactionResponse>();
            var addedTransactions = new List<Transaction>();

            foreach (var transaction in transactions)
            {
                var newTransaction = Mapper.Map<Transaction>(transaction);
                newTransaction.Id = Guid.NewGuid();
                newTransaction.UserId = CurrentAccount.Id;

                addedTransactions.Add(Mapper.Map<Transaction>(newTransaction));
                response.Add(Mapper.Map<TransactionResponse>(newTransaction));
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

            request.Adapt(transaction);

            _TransactionManager.Update(transaction);
            await UnitOfWork.SaveChangesAsync();

            return Mapper.Map<UpdateTransactionResponse>(transaction);
        }

        public async Task<UpdateManyTransactionsResponse> UpdateMany(UpdateManyTransactionsRequest request)
        {
            var requestTransactions = request.Transactions;
            var updatedTransactions = new List<Transaction>();
            var response = new List<TransactionResponse>();

            foreach (var transaction in requestTransactions)
            {
                var existingTransaction = _TransactionManager.FindBy(t => t.Id == transaction.Id).FirstOrDefault();

                if (existingTransaction == null)
                {
                    throw new Exception($"Transaction with Id {transaction.Id} not found");
                }

                transaction.Adapt(existingTransaction);

                updatedTransactions.Add(Mapper.Map<Transaction>(existingTransaction));
                response.Add(Mapper.Map<TransactionResponse>(existingTransaction));
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
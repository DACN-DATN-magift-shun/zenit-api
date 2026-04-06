using MediatR;

using Zenit.Management.Business.Services.TransactionServices;
using Zenit.Management.Contract.TransactionRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class DeleteTransactionHandler(TransactionService transactionService) : IRequestHandler<DeleteTransactionRequest>
    {
        public async Task Handle(DeleteTransactionRequest request, CancellationToken cancellationToken)
        {
            await transactionService.Delete(request);
        }
    }
}
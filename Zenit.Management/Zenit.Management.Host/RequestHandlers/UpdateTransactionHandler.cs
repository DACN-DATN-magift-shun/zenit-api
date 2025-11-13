using MediatR;

using Zenit.Management.Business.Services.TransactionService;
using Zenit.Management.Contract.TransactionRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class UpdateTransactionHandler(TransactionService transactionService) : IRequestHandler<UpdateTransactionRequest, UpdateTransactionResponse>
    {
        public async Task<UpdateTransactionResponse> Handle(UpdateTransactionRequest request, CancellationToken cancellationToken)
        {
            return await transactionService.Update(request);
        }
    }
}
using MediatR;

using Zenit.Management.Business.Services.TransactionService;
using Zenit.Management.Contract.TransactionRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class CreateTransactionHandler(TransactionService transactionService) : IRequestHandler<CreateTransactionRequest, UpdateTransactionResponse>
    {
        public async Task<UpdateTransactionResponse> Handle(CreateTransactionRequest request, CancellationToken cancellationToken)
        {
            return await transactionService.Create(request);
        }
    }
}
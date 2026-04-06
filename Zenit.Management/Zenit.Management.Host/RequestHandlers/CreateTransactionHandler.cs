using MediatR;

using Zenit.Management.Business.Services.TransactionServices;
using Zenit.Management.Contract.TransactionRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class CreateTransactionHandler(TransactionService transactionService) : IRequestHandler<CreateTransactionRequest, CreateTransactionResponse>
    {
        public async Task<CreateTransactionResponse> Handle(CreateTransactionRequest request, CancellationToken cancellationToken)
        {
            return await transactionService.Create(request);
        }
    }
}
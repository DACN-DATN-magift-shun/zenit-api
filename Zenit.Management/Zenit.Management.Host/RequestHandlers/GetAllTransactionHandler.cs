using MediatR;

using Zenit.Management.Business.Services.TransactionServices;
using Zenit.Management.Contract.TransactionRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class GetAllTransactionHandler(TransactionService transactionService) : IRequestHandler<GetAllTransactionRequest, GetAllTransactionResponse>
    {
        public async Task<GetAllTransactionResponse> Handle(GetAllTransactionRequest request, CancellationToken cancellationToken)
        {
            return await transactionService.GetAll(request);
        }
    }
}
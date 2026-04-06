using MediatR;

using Zenit.Management.Business.Services.TransactionServices;
using Zenit.Management.Contract.TransactionRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class GetDetailTransactionHandler(TransactionService transactionService) : IRequestHandler<GetDetailTransactionRequest, GetDetailTransactionResponse>
    {
        public Task<GetDetailTransactionResponse> Handle(GetDetailTransactionRequest request, CancellationToken cancellationToken)
        {
            return transactionService.GetDetail(request);
        }
    }
}
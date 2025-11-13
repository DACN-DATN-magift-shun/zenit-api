using MediatR;

using Zenit.Management.Business.Services.TransactionService;
using Zenit.Management.Contract.TransactionRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class UpdateManyTransactionsHandler(TransactionService transactionService) : IRequestHandler<UpdateManyTransactionsRequest, UpdateManyTransactionsResponse>
    {
        public async Task<UpdateManyTransactionsResponse> Handle(UpdateManyTransactionsRequest request, CancellationToken cancellationToken)
        {
            return await transactionService.UpdateMany(request);
        }
    }
}
using MediatR;

using Zenit.Management.Business.Services.TransactionServices;
using Zenit.Management.Contract.TransactionRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class CreateManyTransactionHandler(TransactionService TransactionService) : IRequestHandler<CreateManyTransactionsRequest, CreateManyTransactionsResponse>
    {
        public async Task<CreateManyTransactionsResponse> Handle(CreateManyTransactionsRequest request, CancellationToken cancellationToken)
        {
            return await TransactionService.CreateMany(request);
        }
    }
}
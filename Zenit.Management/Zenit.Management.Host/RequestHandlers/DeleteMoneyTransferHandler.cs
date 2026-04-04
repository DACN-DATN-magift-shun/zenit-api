using MediatR;

using Zenit.Management.Business.Services;
using Zenit.Management.Contract.Request.MoneyTransferRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class DeleteMoneyTransferHandler(MoneyTransferService moneyTransferService) : IRequestHandler<DeleteMoneyTransferRequest>
    {
        public async Task Handle(DeleteMoneyTransferRequest request, CancellationToken cancellationToken)
        {
            await moneyTransferService.Delete(request);
        }
    }
}
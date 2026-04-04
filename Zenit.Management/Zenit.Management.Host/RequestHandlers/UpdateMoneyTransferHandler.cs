using MediatR;

using Zenit.Management.Business.Services;
using Zenit.Management.Contract.Request.MoneyTransferRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class UpdateMoneyTransferHandler(MoneyTransferService moneyTransferService) : IRequestHandler<UpdateMoneyTransferRequest, UpdateMoneyTransferResponse>
    {
        public async Task<UpdateMoneyTransferResponse> Handle(UpdateMoneyTransferRequest request, CancellationToken cancellationToken)
        {
            return await moneyTransferService.Update(request);
        }
    }
}
using MediatR;

using Zenit.Management.Business.Services;
using Zenit.Management.Contract.Request.MoneyTransferRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class GetDetailMoneyTransferHandler(MoneyTransferService moneyTransferService) : IRequestHandler<GetDetailMoneyTransferRequest, GetDetailMoneyTransferResponse>
    {
        public async Task<GetDetailMoneyTransferResponse> Handle(GetDetailMoneyTransferRequest request, CancellationToken cancellationToken)
        {
            return await moneyTransferService.GetDetail(request);
        }
    }
}
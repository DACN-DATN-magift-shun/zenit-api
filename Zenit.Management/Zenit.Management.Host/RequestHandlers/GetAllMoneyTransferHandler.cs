using MediatR;

using Zenit.Management.Business.Services;
using Zenit.Management.Contract.Request.MoneyTransferRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class GetAllMoneyTransferHandler(MoneyTransferService moneyTransferService) : IRequestHandler<GetAllMoneyTransferRequest, GetAllMoneyTransferResponse>
    {
        public async Task<GetAllMoneyTransferResponse> Handle(GetAllMoneyTransferRequest request, CancellationToken cancellationToken)
        {
            return await moneyTransferService.GetAll(request);
        }
    }
}
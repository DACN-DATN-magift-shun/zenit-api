using MediatR;

using Zenit.Management.Business.Services.WalletServices;
using Zenit.Management.Contract.Request.MoneyTransferRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class CreateMoneyTransferHandler(MoneyTransferService moneyTransferService) : IRequestHandler<CreateMoneyTransferRequest, CreateMoneyTransferResponse>
    {
        public async Task<CreateMoneyTransferResponse> Handle(CreateMoneyTransferRequest request, CancellationToken cancellationToken)
        {
            return await moneyTransferService.Create(request);
        }
    }
}
using MediatR;

using Zenit.Management.Business.Services.WalletServices;
using Zenit.Management.Contract.Request.WalletRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class CreateWalletHandler(WalletService walletService) : IRequestHandler<CreateWalletRequest, CreateWalletResponse>
    {
        public async Task<CreateWalletResponse> Handle(CreateWalletRequest request, CancellationToken cancellationToken)
        {
            return await walletService.Create(request);
        }
    }
}
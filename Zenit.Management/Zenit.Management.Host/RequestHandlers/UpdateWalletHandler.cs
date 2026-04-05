using MediatR;

using Zenit.Management.Business.Services.WalletServices;
using Zenit.Management.Contract.Request.WalletRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class UpdateWalletHandler(WalletService walletService) : IRequestHandler<UpdateWalletRequest, UpdateWalletResponse>
    {
        public async Task<UpdateWalletResponse> Handle(UpdateWalletRequest request, CancellationToken cancellationToken)
        {
            return await walletService.Update(request);
        }
    }
}
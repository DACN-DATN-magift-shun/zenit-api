using MediatR;

using Zenit.Management.Business.Services;
using Zenit.Management.Contract.Request.WalletRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class GetDetailWalletHandler(WalletService walletService) : IRequestHandler<GetDetailWalletRequest, GetDetailWalletResponse>
    {
        public async Task<GetDetailWalletResponse> Handle(GetDetailWalletRequest request, CancellationToken cancellationToken)
        {
            return await walletService.GetDetail(request);
        }
    }
}
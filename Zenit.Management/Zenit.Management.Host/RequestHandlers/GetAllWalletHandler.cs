using MediatR;

using Zenit.Management.Business.Services;
using Zenit.Management.Contract.Request.WalletRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class GetAllWalletHandler(WalletService walletService) : IRequestHandler<GetAllWalletRequest, GetAllWalletResponse>
    {
        public async Task<GetAllWalletResponse> Handle(GetAllWalletRequest request, CancellationToken cancellationToken)
        {
            return await walletService.GetAll(request);
        }
    }
}
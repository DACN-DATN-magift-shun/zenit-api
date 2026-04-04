using MediatR;

using Zenit.Management.Business.Services;
using Zenit.Management.Contract.Request.WalletRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class DeleteWalletHandler(WalletService walletService) : IRequestHandler<DeleteWalletRequest>
    {
        public async Task Handle(DeleteWalletRequest request, CancellationToken cancellationToken)
        {
            await walletService.Delete(request);
        }
    }
}
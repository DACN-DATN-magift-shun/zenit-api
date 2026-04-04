using MediatR;

namespace Zenit.Management.Contract.Request.WalletRequests
{
    public class DeleteWalletRequest : IRequest
    {
        public Guid Id { get; set; }
    }
}
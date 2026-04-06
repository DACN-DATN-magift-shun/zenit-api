using MediatR;

namespace Zenit.Management.Contract.Request.WalletRequests
{
    public class GetDetailWalletRequest : IRequest<GetDetailWalletResponse>
    {
        public Guid Id { get; set; }
    }

    public class GetDetailWalletResponse
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required int Amount { get; set; }
        public required string BackgroundColor { get; set; }
        public required string Icon { get; set; }
        public required string Note { get; set; }
        public required bool IsIncludeInTotalBalance { get; set; }
    }
}
using MediatR;

namespace Zenit.Management.Contract.Request.WalletRequests
{
    public class UpdateWalletRequest : IRequest<UpdateWalletResponse>
    {
        public required Guid Id { get; set; }
        public string? Name { get; set; }
        public int? Amount { get; set; }
        public string? BackgroundColor { get; set; }
        public string? Icon { get; set; }
        public string? Note { get; set; }
        public bool? IsIncludeInTotalBalance { get; set; }
    }

    public class UpdateWalletResponse
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
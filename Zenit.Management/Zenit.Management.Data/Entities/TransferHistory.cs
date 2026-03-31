using Zenit.Management.Data.Models;

namespace Zenit.Management.Data.Entities
{
    public class TransferHistory : ManagementAuditModel
    {
        public required Guid FromWalletId { get; set; }
        public required Guid ToWalletId { get; set; }
        public required int Amount { get; set; }
        public required DateTime TransferDate { get; set; }
        public string? Note { get; set; }
        public virtual Wallet? FromWallet { get; set; }
        public virtual Wallet? ToWallet { get; set; }
        public required Guid AccountId { get; set; }
    }
}
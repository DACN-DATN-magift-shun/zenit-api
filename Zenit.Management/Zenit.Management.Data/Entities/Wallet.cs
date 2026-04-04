using Zenit.Management.Data.Models;

namespace Zenit.Management.Data.Entities
{
    public class Wallet : ManagementAuditModel
    {
        public required string Name { get; set; }
        public required int Amount { get; set; }
        public required string BackgroundColor { get; set; }
        public required string Icon { get; set; }
        public string? Note { get; set; }
        public bool IsIncludeInTotalBalance { get; set; } = true;
        public required Guid AccountId { get; set; }
    }
}
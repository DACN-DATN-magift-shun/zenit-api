using Zenit.Management.Data.Models;

namespace Zenit.Management.Data.Entities
{
    public class Transaction : ManagementAuditModel
    {
        public required string Title { get; set; }
        public string? Note { get; set; }
        public required int Amount { get; set; }
        public required DateTime TransactionDate { get; set; }
        public required Guid CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public required Guid UserId { get; set; }
    }
}
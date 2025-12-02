using Zenit.Management.Data.Models;

namespace Zenit.Management.Data.Entities
{
    public class CategorySettings : ManagementAuditModel
    {
        public required Guid AccountId { get; set; }
        public required Guid CategoryId { get; set; }
        public int? ExpenseLimit { get; set; }
        public float? ExpenseAlertThreshold { get; set; }
    }
}
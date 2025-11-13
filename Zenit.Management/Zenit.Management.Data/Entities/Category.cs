using Zenit.Management.Data.Models;

namespace Zenit.Management.Data.Entities
{
    public class Category : ManagementAuditModel
    {
        public required string Name { get; set; }
        public required string Icon { get; set; }
        public long? ExpenseLimit { get; set; }
        public float? ExpenseAlertThreshold { get; set; }
        public required Guid GroupId { get; set; }
        public required Guid UserId { get; set; }
    }
}
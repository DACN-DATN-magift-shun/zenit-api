using Zenit.Management.Data.Models;

namespace Zenit.Management.Data.Entities
{
    public class Category : ManagementAuditModel
    {
        public required string Name { get; set; }
        public required string Icon { get; set; }
        public string? ExpenseLimit { get; set; }
        public int? LimitAlertThreshold { get; set; }
        public required Guid GroupId { get; set; }
    }
}
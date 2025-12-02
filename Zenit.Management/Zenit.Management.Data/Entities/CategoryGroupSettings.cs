using Zenit.Management.Data.Models;
using Zenit.Share.Common.Enums;

namespace Zenit.Management.Data.Entities
{
    public class CategoryGroupSettings : ManagementAuditModel
    {
        public required CategoryGroupType GroupType { get; set; }
        public float? ExpenseLimit { get; set; }
        public float? ExpenseAlertThreshold { get; set; }
        public required Guid AccountId { get; set; }
    }
}
using Zenit.Share.Common.Enums;

namespace Zenit.Management.Data.Entities
{
    public class CategoryGroupExpenseAlertThreshold
    {
        public required Guid Id { get; set; }
        public required CategoryGroupType GroupType { get; set; }
        public float? Threshold { get; set; }
        public required Guid UserId { get; set; }
    }
}
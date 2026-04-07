using Zenit.Management.Common.Enums;
using Zenit.Management.Data.Models;

namespace Zenit.Management.Data.Entities
{
    public class Goal : ManagementAuditModel
    {
        public required string Name { get; set; }
        public required int TargetAmount { get; set; }
        public int CurrentAmount { get; set; } = 0;
        public required string BackgroundColor { get; set; }
        public required string Icon { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Note { get; set; }
        public required GoalStatus Status { get; set; }
        public required Guid AccountId { get; set; }
    }
}
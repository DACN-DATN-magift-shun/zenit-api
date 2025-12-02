using Zenit.Management.Data.Models;
using Zenit.Share.Common.Enums;

namespace Zenit.Management.Data.Entities
{
    public class Category : ManagementAuditModel
    {
        public required string Name { get; set; }
        public required string Icon { get; set; }
        public required string Color { get; set; }
        public required string BackgroundColor { get; set; }
        public required CategoryGroupType GroupType { get; set; }
        public Guid? AccountId { get; set; }
    }
}
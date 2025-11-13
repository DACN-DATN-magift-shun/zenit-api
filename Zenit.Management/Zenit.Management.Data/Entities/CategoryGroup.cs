using Zenit.Management.Data.Models;
using Zenit.Share.Common.Enums;

namespace Zenit.Management.Data.Entities
{
    public class CategoryGroup : ManagementAuditModel
    {
        public required string Name { get; set; }
        public required CategoryGroupType GroupType { get; set; }
    }
}
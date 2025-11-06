using System.Globalization;

using Zenit.Management.Common.Enums;
using Zenit.Management.Data.Models;

namespace Zenit.Management.Data.Entities
{
    public class CategoryGroup : ManagementAuditModel
    {
        public required string Name { get; set; }
        public required CategoryGroupType GroupType { get; set; }
        public int? LimitAlertThreshold { get; set; }
    }
}
using Microsoft.EntityFrameworkCore;

using Zenit.Share.Common.Values;

namespace Zenit.Share.Data.Models
{
    public class ChangedEntity
    {
        public required object Entity { get; set; }
        public List<AuditDataChange>? DataChanges { get; set; }
        public EntityState State { get; set; }
    }
}
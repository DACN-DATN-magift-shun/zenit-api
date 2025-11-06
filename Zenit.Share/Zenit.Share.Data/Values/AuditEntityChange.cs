using Microsoft.EntityFrameworkCore;

using Zenit.Share.Common.Values;

namespace Zenit.Share.Data.Values
{
    public class AuditEntityChange
    {
        public EntityState State { get; set; }
        public object? Entity { get; set; }
        public List<AuditFieldChange>? FieldChanges { get; set; }
    }
}
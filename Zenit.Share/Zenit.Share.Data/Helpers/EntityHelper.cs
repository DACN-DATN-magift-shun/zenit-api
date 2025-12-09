using Microsoft.EntityFrameworkCore.ChangeTracking;

using Zenit.Share.Common.Values;

namespace Zenit.Share.Data.Helpers
{
    public class EntityHelper
    {
        public static List<AuditDataChange> GetDataChanges(EntityEntry entityEntry)
        {
            var changes = new List<AuditDataChange>();

            foreach (var property in entityEntry.OriginalValues.Properties)
            {
                var field = property.Name;
                var fieldOriginalValue = entityEntry.OriginalValues?[property];
                var fieldCurrentValue = entityEntry.CurrentValues?[property];

                changes.Add(new AuditDataChange
                {
                    Field = field,
                    OriginalValue = fieldOriginalValue,
                    NewValue = fieldCurrentValue,
                });
            }

            return changes;
        }
    }
}
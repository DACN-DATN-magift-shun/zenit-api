// using Microsoft.EntityFrameworkCore.ChangeTracking;

// using Zenit.Share.Common.Values;

// namespace Zenit.Share.Data.Helpers
// {
//     public static class EntityHelper
//     {
//         public static List<AuditFieldChange> GetAuditFieldChanges(EntityEntry entity)
//         {
//             var changes = new List<AuditFieldChange>();

//             foreach (var property in entity.Properties)
//             {
//                 changes.Add(new AuditFieldChange
//                 {
//                     Field = property.Metadata.Name,
//                     OriginValue = property.OriginalValue?.ToString(),
//                     NewValue = property.CurrentValue?.ToString()
//                 });
//             }
            
//             return changes;
//         }
//     }
// }
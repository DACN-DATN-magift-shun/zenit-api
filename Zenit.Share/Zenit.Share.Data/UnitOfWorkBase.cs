using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

// using Zenit.Share.Data.Events.Requests;
// using Zenit.Share.Data.Helpers;
using Zenit.Share.Data.Interfaces;
// using Zenit.Share.Data.Values;


namespace Zenit.Share.Data
{
    public abstract class UnitOfWorkBase<TContext>(
        TContext context,
        IPublisher publisher
    ) : IUnitOfWork
        where TContext : DbContext
    {
        public async Task BeginTransactionAsync()
        {
            await context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await context.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await context.Database.RollbackTransactionAsync();
        }

        public virtual void Dispose()
        {
            context.Database.CurrentTransaction?.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task SaveChangesAsync()
        {
            // var states = new[] {
            //     EntityState.Added,
            //     EntityState.Modified,
            //     EntityState.Deleted
            // };

            // var entries = context.ChangeTracker.Entries()
            //     .Where(e => states.Contains(e.State))
            //     .ToList();

            // var changedEntities = entries.Where(e => e.Entity is IDataModel)
            //     .Select(e => new AuditEntityChange
            //     {
            //         State = e.State,
            //         Entity = e.CurrentValues.ToObject(),
            //         FieldChanges = EntityHelper.GetAuditFieldChanges(e)
            //     })
            //     .ToList();

            await context.SaveChangesAsync();

            // await PublishEventRequestsAsync(changedEntities);
        }

        // private async Task PublishEventRequestsAsync(List<AuditEntityChange> changedEntities)
        // {
        //     try
        //     {
        //         foreach (var entity in changedEntities)
        //         {
        //             var IsDeleted = entity.FieldChanges?.FirstOrDefault(e => e.Field == "IsDeleted")?.NewValue;

        //             if (entity.State == EntityState.Added)
        //             {
        //                 var entityEventType = typeof(EntityCreationEventRequest<>)
        //                                     .MakeGenericType(entity.Entity!.GetType());
        //                 await publisher.Publish(
        //                     Activator.CreateInstance(entityEventType, entity.Entity, entity.FieldChanges)!
        //                 );
        //             }
        //             else if (entity.State == EntityState.Modified && IsDeleted!.Equals(true))
        //             {
        //                 var entityEventType = typeof(EntityDeletionEventRequest<>)
        //                                     .MakeGenericType(entity.Entity!.GetType());
        //                 await publisher.Publish(
        //                     Activator.CreateInstance(entityEventType, entity.Entity, entity.FieldChanges)!
        //                 );
        //             }
        //             else if (entity.State == EntityState.Modified && IsDeleted!.Equals(false))
        //             {
        //                 var entityEventType = typeof(EntityModificationEventRequest<>)
        //                                     .MakeGenericType(entity.Entity!.GetType());
        //                 await publisher.Publish(
        //                     Activator.CreateInstance(entityEventType, entity.Entity, entity.FieldChanges)!
        //                 );
        //             }

        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine(ex);
        //     }
        // }
    }
}

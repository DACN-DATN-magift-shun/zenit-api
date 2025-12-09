using MediatR;

using Microsoft.EntityFrameworkCore;

using Zenit.Share.Data.Events.Requests;
using Zenit.Share.Data.Helpers;



// using Zenit.Share.Data.Events.Requests;
// using Zenit.Share.Data.Helpers;
using Zenit.Share.Data.Interfaces;
using Zenit.Share.Data.Models;
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
            var states = new[] {
                EntityState.Added,
                EntityState.Modified,
                EntityState.Deleted
            };

            var entries = context.ChangeTracker.Entries()
                .Where(e => states.Contains(e.State))
                .ToList();

            var changedEntities = entries.Where(e => e.Entity is IDataModel)
                .Select(e => new ChangedEntity
                {
                    Entity = e.CurrentValues.ToObject(),
                    DataChanges = EntityHelper.GetDataChanges(e),
                    State = e.State
                })
                .ToList();

            await context.SaveChangesAsync();

            await PublishEventRequestsAsync(changedEntities);
        }

        private async Task PublishEventRequestsAsync(List<ChangedEntity> changedEntities)
        {
            try
            {
                foreach (var entity in changedEntities)
                {
                    var deletedById = entity.Entity.GetType().GetProperty("DeletedById").GetValue(entity.Entity);
                    if (entity.State == EntityState.Added)
                    {
                        await publisher.Publish(new EventCreateRequest<IDataModel>
                        {
                            Data = (IDataModel)entity.Entity,
                            DataChanges = entity.DataChanges,
                        });
                    }
                    else if (entity.State == EntityState.Modified && deletedById == null)
                    {
                        await publisher.Publish(new EventUpdateRequest<IDataModel>
                        {
                            Data = (IDataModel)entity.Entity,
                            DataChanges = entity.DataChanges,
                        });
                    }
                    else if (entity.State == EntityState.Deleted || deletedById != null)
                    {
                        await publisher.Publish(new EventDeleteRequest<IDataModel>
                        {
                            Data = (IDataModel)entity.Entity,
                            DataChanges = entity.DataChanges,
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using Zenit.Share.Data.Interfaces;
using Zenit.Share.Data.Models;


namespace Zenit.Share.Data
{
    public abstract class UnitOfWorkBase<TContext>(
        TContext context
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
            var states = new List<EntityState> { EntityState.Added, EntityState.Modified, EntityState.Deleted };

            var entries = context.ChangeTracker.Entries()
                .Where(e => states.Contains(e.State))
                .ToList();
            
            var changedEntities = entries.Where(e => e.Entity is IDataModel)
            .Select(e => new changedEntity
            {
                EntityCurrentValues = e.CurrentValues.ToObject(),
                EntityState = e.State
            }).ToList();

            await context.SaveChangesAsync();

            await AfterSaveChangesAsync(changedEntities);
        }

        public async Task AfterSaveChangesAsync(List<changedEntity> changedEntities)
        {
            if (changedEntities == null)
                return;

            foreach (var changedEntity in changedEntities)
            {
                // Implement custom logic for each changed entity
            }
        }
    }
}

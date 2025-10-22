using Microsoft.EntityFrameworkCore;

using Zenit.Share.Data.Interfaces;


namespace Zenit.Share.Data
{
    public abstract class UnitOfWorkBase<TContext>(TContext context) : IUnitOfWork
        where TContext : DbContext
    {
        public async Task SaveChangesAsync()
        {
            await Task.CompletedTask;
            // implement event sourcing here
        }

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

    }
}

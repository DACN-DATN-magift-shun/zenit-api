using Zenit.Share.Data.Interfaces;


namespace Zenit.Share.Data
{
    public abstract class UnitOfWorkBase<TContext>(TContext context) : IUnitOfWork
        where TContext : class
    {
        public abstract void SaveChanges();
        public abstract void BeginTransaction();
        public abstract void CommitTransaction();
        public abstract void RollbackTransaction();
        public abstract void Dispose();
    }
}

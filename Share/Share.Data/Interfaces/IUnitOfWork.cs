namespace Share.Data.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        Task SaveChanges();
        Task BeginTransaction();
        Task CommitTransaction();
        Task RollbackTransaction();
    }
}
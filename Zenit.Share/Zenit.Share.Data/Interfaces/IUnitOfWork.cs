namespace Zenit.Share.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}

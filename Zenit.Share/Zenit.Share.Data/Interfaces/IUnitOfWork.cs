namespace Zenit.Share.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}

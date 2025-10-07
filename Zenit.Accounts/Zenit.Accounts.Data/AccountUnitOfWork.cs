using MongoDB.Driver;
using Zenit.Share.Data;


namespace Zenit.Accounts.Data
{
    public class AccountUnitOfWork(AccountDbContext context) : UnitOfWorkBase<AccountDbContext>(context)
    {
        private readonly AccountDbContext _context = context;
        private IClientSessionHandle _session;
        private bool _disposed = false;

        public override void BeginTransaction()
        {
            _session = _context.GetDatabase().Client.StartSession();
            _session.StartTransaction();
        }

        public override void CommitTransaction()
        {
            _session.CommitTransaction();
        }

        public override void RollbackTransaction()
        {
            _session.AbortTransaction();
        }

        public override void Dispose()
        {
            if (!_disposed)
            {
                _session.Dispose();
                _disposed = true;
            }

            GC.SuppressFinalize(this);
        }

        public override void SaveChanges()
        {
            // need no implementation
        }
    }
}

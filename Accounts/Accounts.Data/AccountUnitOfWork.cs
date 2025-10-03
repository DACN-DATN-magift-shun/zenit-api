using MongoDB.Driver;
using Share.Data;


namespace Accounts.Data
{
    public class AccountUnitOfWork(AccountDbContext context) : UnitOfWorkBase<AccountDbContext>(context)
    {
        private readonly AccountDbContext _context = context;
        private IClientSessionHandle? _session;
        private bool _disposed = false;
        public override Task BeginTransaction()
        {
            _session = _context.GetDatabase().Client.StartSession();
            _session.StartTransaction();

            return Task.CompletedTask;
        }
        public override async Task CommitTransaction()
        {
            if (_session == null)
            {
                throw new InvalidOperationException("No transaction started.");
            }

            try
            {
                await _session.CommitTransactionAsync();
            }
            finally
            {
                _session.Dispose();
                _session = null;
            }
        }
        public override async Task RollbackTransaction()
        {
            if (_session == null)
            {
                throw new InvalidOperationException("No transaction started.");
            }

            try
            {
                await _session.AbortTransactionAsync();
            }
            finally
            {
                _session.Dispose();
                _session = null;
            }
        }
        public override async ValueTask DisposeAsync()
        {
            if (!_disposed)
            {
                if(_session != null)
                {
                    _session.Dispose();
                    _session = null;
                }
                _disposed = true;
            }

            GC.SuppressFinalize(this);
            await Task.CompletedTask;
        }
        public override Task SaveChanges()
        {
            return Task.CompletedTask;
        }
    }
}
using Zenit.Accounts.Common.Models;
using Zenit.Share.Data;

namespace Zenit.Accounts.Data
{
    public class AccountRepository<TEntity>(
        AccountDbContext context,
        CurrentAccount currentAccount) : RepositoryBase<TEntity, Guid>(context, currentAccount.Id)
        where TEntity : class
    {
    }
}

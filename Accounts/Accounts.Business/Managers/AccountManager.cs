using Accounts.Data.Schemas;
using Share.Data.Interfaces;


namespace Accounts.Business.Managers
{
    public class AccountManager(IRepository<Account> repository) : AccountDomainService<Account>(repository)
    {

    }
}
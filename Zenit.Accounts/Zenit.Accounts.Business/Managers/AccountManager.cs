using Zenit.Accounts.Data.Schemas;
using Zenit.Share.Data.Interfaces;


namespace Zenit.Accounts.Business.Managers
{
    public class AccountManager(IRepository<Account> repository) : AccountDomainService<Account>(repository)
    {

    }
}

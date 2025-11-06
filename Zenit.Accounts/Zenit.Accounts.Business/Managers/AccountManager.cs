using Zenit.Accounts.Data;
using Zenit.Accounts.Data.Entities;
using Zenit.Share.Data.Interfaces;


namespace Zenit.Accounts.Business.Managers
{
    public class AccountManager(
        AccountRepository<Account> repository) : AccountDomainService<Account>(repository)
    {
    }
}

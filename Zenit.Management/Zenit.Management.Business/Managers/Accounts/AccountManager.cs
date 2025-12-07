using Zenit.Management.Data.Entities;
using Zenit.Share.Data.Interfaces;

namespace Zenit.Management.Business.Managers
{
    public class AccountManager(
        IRepository<Account> repository) : ManagementDomainService<Account>(repository)
    {
    }
}

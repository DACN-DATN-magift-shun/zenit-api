using Zenit.Management.Data.Entities;
using Zenit.Share.Data.Interfaces;

namespace Zenit.Management.Business.Managers
{
    public class MoneyTransferManager(IRepository<MoneyTransferHistory> repository) : ManagementDomainService<MoneyTransferHistory>(repository)
    {
    }
}
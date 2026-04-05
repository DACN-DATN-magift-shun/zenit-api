using Zenit.Management.Data.Entities;
using Zenit.Share.Data.Interfaces;

namespace Zenit.Management.Business.Managers.WalletManager
{
    public class MoneyTransferManager(IRepository<MoneyTransferHistory> repository) : ManagementDomainService<MoneyTransferHistory>(repository)
    {
    }
}
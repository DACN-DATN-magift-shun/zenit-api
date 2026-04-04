using Zenit.Management.Data.Entities;
using Zenit.Share.Data.Interfaces;

namespace Zenit.Management.Business.Managers
{
    public class WalletManager(IRepository<Wallet> repository) : ManagementDomainService<Wallet>(repository)
    {
    }
}
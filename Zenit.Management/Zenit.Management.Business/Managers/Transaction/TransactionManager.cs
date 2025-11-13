using Zenit.Management.Data;
using Zenit.Management.Data.Entities;
using Zenit.Share.Data.Interfaces;

namespace Zenit.Management.Business.Managers
{
    public class TransactionManager(IRepository<Transaction> repository) : ManagementDomainService<Transaction>(repository)
    {
    }
}
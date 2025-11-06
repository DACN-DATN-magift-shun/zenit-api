using Zenit.Management.Common.Models;
using Zenit.Share.Data;

namespace Zenit.Management.Data
{
    public class ManagementRepository<TEntity>(
        ManagementDbContext dbContext,
        ManagementCurrentAccount currentAccount
    ) : RepositoryBase<TEntity, Guid>(dbContext, currentAccount.Id)
        where TEntity : class
    {  
    }
}
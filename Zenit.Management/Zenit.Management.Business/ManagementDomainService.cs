using Zenit.Management.Data;
using Zenit.Share.Business;
using Zenit.Share.Data.Interfaces;

namespace Zenit.Management.Business
{
    public class ManagementDomainService<TEntity>(IRepository<TEntity> repository) : DomainServiceBase<TEntity>(repository)
        where TEntity : class
    {
    }
}
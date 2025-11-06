using Zenit.Management.Data;
using Zenit.Share.Business;

namespace Zenit.Management.Business
{
    public class ManagementDomainService<TEntity>(ManagementRepository<TEntity> repository) : DomainServiceBase<TEntity>(repository)
        where TEntity : class
    {
    }
}
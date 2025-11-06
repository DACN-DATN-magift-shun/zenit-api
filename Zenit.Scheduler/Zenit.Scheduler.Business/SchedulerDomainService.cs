using Zenit.Scheduler.Data;
using Zenit.Share.Business;

namespace Zenit.Scheduler.Business
{
    public class SchedulerDomainService<TEntity>(
        SchedulerRepository<TEntity> repository
    ) : DomainServiceBase<TEntity>(repository)
        where TEntity : class
    {
    }
}
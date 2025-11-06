using Zenit.Share.Data;
using Zenit.Share.Data.Models;

namespace Zenit.Scheduler.Data
{
    public class SchedulerRepository<TEntity>(
        SchedulerDbContext context
    ) : RepositoryBase<TEntity, Guid>(context, null)
        where TEntity : class
    {
    }
}
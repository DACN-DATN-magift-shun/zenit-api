using Zenit.Scheduler.Data;
using Zenit.Scheduler.Data.Entities;

namespace Zenit.Scheduler.Business.Managers
{
    public class JobManager(SchedulerRepository<Job> repository) : SchedulerDomainService<Job>(repository)
    {
    }
}
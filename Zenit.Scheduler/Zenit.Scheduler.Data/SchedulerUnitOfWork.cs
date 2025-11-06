using MediatR;

using Zenit.Share.Data;

namespace Zenit.Scheduler.Data
{
    public class SchedulerUnitOfWork(
        SchedulerDbContext context,
        IPublisher publisher) : UnitOfWorkBase<SchedulerDbContext>(context, publisher)
    {
    }
}
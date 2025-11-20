using Zenit.Share.Data;

namespace Zenit.Statistics.Data
{
    public class StatisticsUnitOfWork(StatisticsDbContext dbContext) 
        : UnitOfWorkBase<StatisticsDbContext>(dbContext)
    {
    }
}
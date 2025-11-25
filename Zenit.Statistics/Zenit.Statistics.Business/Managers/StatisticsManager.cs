using Zenit.Share.Data.Interfaces;
using Zenit.Statistics.Data.Entities;

namespace Zenit.Statistics.Business.Managers
{
    public class StatisticsManager(IRepository<CategoryDailyStatistics> repository) : StatisticsDomainService<CategoryDailyStatistics>(repository)
    {
    }
}
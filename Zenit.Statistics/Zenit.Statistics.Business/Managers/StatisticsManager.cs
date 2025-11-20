using Zenit.Share.Data.Interfaces;
using Zenit.Statistics.Data.Entities;

namespace Zenit.Statistics.Business.Managers
{
    public class StatisticsManager(IRepository<TransactionStatistics> repository) : StatisticsDomainService<TransactionStatistics>(repository)
    {
    }
}
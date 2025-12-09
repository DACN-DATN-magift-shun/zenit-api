using Zenit.Share.Data.Interfaces;
using Zenit.Management.Data.Entities;

namespace Zenit.Management.Business.Managers
{
    public class StatisticsManager(IRepository<CategoryDailyStatistics> repository) : ManagementDomainService<CategoryDailyStatistics>(repository)
    {
    }
}
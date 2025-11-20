using Zenit.Share.Data;
using Zenit.Statistics.Common.Models;

namespace Zenit.Statistics.Data
{
    public class StatisticsRepository<TEntity>(StatisticsDbContext dbContext, StatisticsCurrentAccount currentAccount) 
        : RepositoryBase<TEntity, Guid>(dbContext, currentAccount.Id) where TEntity : class
    {
    }
}
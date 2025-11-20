using Zenit.Share.Business;
using Zenit.Share.Data.Interfaces;


namespace Zenit.Statistics.Business
{
    public class StatisticsDomainService<TEntity>(IRepository<TEntity> repository)
        : DomainServiceBase<TEntity>(repository)
        where TEntity : class
    {
    }
}
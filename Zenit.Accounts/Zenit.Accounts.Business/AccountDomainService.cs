using Zenit.Share.Business;
using Zenit.Share.Data.Interfaces;


namespace Zenit.Accounts.Business
{
    public class AccountDomainService<TEntity>(IRepository<TEntity> repository) : DomainServiceBase<TEntity>(repository)
        where TEntity : class
    {

    }
}

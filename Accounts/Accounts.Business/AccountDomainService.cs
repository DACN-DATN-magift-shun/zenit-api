using Share.Business;
using Share.Data.Interfaces;


namespace Accounts.Business
{
    public class AccountDomainService<TSchema>(IRepository<TSchema> repository) : DomainServiceBase<TSchema>(repository)
        where TSchema : class
    {

    }
}
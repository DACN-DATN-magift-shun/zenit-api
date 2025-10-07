using Zenit.Share.Business;
using Zenit.Share.Data.Interfaces;


namespace Zenit.Accounts.Business
{
    public class AccountDomainService<TSchema>(IRepository<TSchema> repository) : DomainServiceBase<TSchema>(repository)
        where TSchema : class
    {

    }
}

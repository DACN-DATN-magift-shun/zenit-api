using MediatR;

using Zenit.Share.Data;

namespace Zenit.Accounts.Data
{
    public class AccountUnitOfWork(
        AccountDbContext context,
        IPublisher publisher) : UnitOfWorkBase<AccountDbContext>(context, publisher)
    {
    }
}

using MediatR;

using Zenit.Accounts.Contract.Requests;
using Zenit.Accounts.Business.Services;


namespace Zenit.Accounts.Host.RequestHandlers
{
    public class AccountDeleteHandler(AccountService accountService) : IRequestHandler<AccountDeleteRequest>
    {
        public async Task Handle(AccountDeleteRequest request, CancellationToken cancellationToken)
        {
            await accountService.Delete(request);
        }
    }
}

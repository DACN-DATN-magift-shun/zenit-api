using MediatR;

using Zenit.Accounts.Business.Services;
using Zenit.Accounts.Contract.Requests;


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

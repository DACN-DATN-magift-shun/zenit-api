using MediatR;

using Accounts.Contract.Requests;
using Accounts.Business.Services;


namespace Accounts.Host.RequestHandlers
{
    public class AccountDeleteHandler(AccountService accountService) : IRequestHandler<AccountDeleteRequest>
    {
        public async Task Handle(AccountDeleteRequest request, CancellationToken cancellationToken)
        {
            await accountService.Delete(request);
        }
    }
}
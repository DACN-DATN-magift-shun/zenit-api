using MediatR;

using Accounts.Business.Services;
using Accounts.Contract.Requests;


namespace Accounts.Host.RequestHandlers
{
    public class AccountLoginHandler(AccountService accountService) : IRequestHandler<AccountLoginRequest, AccountLoginResponse>
    {
        public async Task<AccountLoginResponse> Handle(AccountLoginRequest request, CancellationToken cancellationToken)
        {
            return await accountService.Login(request);
        }
    }
}
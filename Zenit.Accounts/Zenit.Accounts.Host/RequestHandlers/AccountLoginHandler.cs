using MediatR;

using Zenit.Accounts.Business.Services;
using Zenit.Accounts.Contract.Requests;


namespace Zenit.Accounts.Host.RequestHandlers
{
    public class AccountLoginHandler(AccountService accountService) : IRequestHandler<AccountLoginRequest, AccountLoginResponse>
    {
        public async Task<AccountLoginResponse> Handle(AccountLoginRequest request, CancellationToken cancellationToken)
        {
            return await accountService.Login(request);
        }
    }
}

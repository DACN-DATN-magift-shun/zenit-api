using MediatR;

using Zenit.Management.Business.Services;
using Zenit.Management.Contract.Requests.AccountRequests;


namespace Zenit.Management.Host.RequestHandlers
{
    public class AccountLoginHandler(AccountService accountService) : IRequestHandler<AccountLoginRequest, AccountLoginResponse>
    {
        public async Task<AccountLoginResponse> Handle(AccountLoginRequest request, CancellationToken cancellationToken)
        {
            return await accountService.Login(request);
        }
    }
}

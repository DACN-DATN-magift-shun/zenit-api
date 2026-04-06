using MediatR;

using Zenit.Management.Business.Services.Accounts;
using Zenit.Management.Contract.Requests.AccountRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class ResetPasswordHandler(AccountService accountService) : IRequestHandler<AccountResetPasswordRequest, AccountResetPasswordResponse>
    {
        public async Task<AccountResetPasswordResponse> Handle(AccountResetPasswordRequest request, CancellationToken cancellationToken)
        {
            return await accountService.ResetPassword(request);
        }
    }
}
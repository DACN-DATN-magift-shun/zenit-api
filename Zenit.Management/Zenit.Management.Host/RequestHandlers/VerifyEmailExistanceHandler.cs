using MediatR;

using Zenit.Management.Business.Services.Accounts;
using Zenit.Management.Contract.Requests.AccountRequests;


namespace Zenit.Management.Host.RequestHandlers
{
    public class VerifyEmailExistanceHandler(AccountService accountService) : IRequestHandler<AccountIsEmailExistRequest, AccountIsEmailExistResponse>
    {
        public async Task<AccountIsEmailExistResponse> Handle(AccountIsEmailExistRequest request, CancellationToken cancellationToken)
        {
            return await accountService.VerifyEmailExistance(request);
        }
    }
}

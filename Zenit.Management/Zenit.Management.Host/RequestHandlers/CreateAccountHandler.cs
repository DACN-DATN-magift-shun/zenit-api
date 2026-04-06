using MediatR;

using Zenit.Management.Business.Services.Accounts;
using Zenit.Management.Contract.Requests.AccountRequests;


namespace Zenit.Management.Host.RequestHandlers
{
    public class AccountCreateHandler(AccountService accountService) : IRequestHandler<AccountCreateRequest, AccountCreateResponse>
    {
        public async Task<AccountCreateResponse> Handle(AccountCreateRequest request, CancellationToken cancellationToken)
        {
            return await accountService.Create(request);
        }
    }
}

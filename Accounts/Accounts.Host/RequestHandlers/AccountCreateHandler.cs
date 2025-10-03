using MediatR;

using Accounts.Business.Services;
using Accounts.Contract.Requests;


namespace Accounts.Host.RequestHandlers
{
    public class AccountCreateHandler(AccountService accountService) : IRequestHandler<AccountCreateRequest, AccountCreateResponse>
    {
        public async Task<AccountCreateResponse> Handle(AccountCreateRequest request, CancellationToken cancellationToken)
        {
            return await accountService.Create(request);
        }
    }
}
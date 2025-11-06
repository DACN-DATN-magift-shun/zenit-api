using MediatR;

using Zenit.Accounts.Business.Services;
using Zenit.Accounts.Contract.Requests;


namespace Zenit.Accounts.Host.RequestHandlers
{
    public class AccountCreateHandler(AccountService accountService) : IRequestHandler<AccountCreateRequest, AccountCreateResponse>
    {
        public async Task<AccountCreateResponse> Handle(AccountCreateRequest request, CancellationToken cancellationToken)
        {
            return await accountService.Create(request);
        }
    }
}

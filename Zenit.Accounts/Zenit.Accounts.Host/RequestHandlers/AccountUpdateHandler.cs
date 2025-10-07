using MediatR;

using Zenit.Accounts.Business.Services;
using Zenit.Accounts.Contract.Requests;


namespace Zenit.Accounts.Host.RequestHandlers
{
    public class AccountUpdateHandler(AccountService accountService) : IRequestHandler<AccountUpdateRequest, AccountUpdateResponse>
    {
        public async Task<AccountUpdateResponse> Handle(AccountUpdateRequest request, CancellationToken cancellationToken)
        {
            return await accountService.Update(request);
        }
    }
}

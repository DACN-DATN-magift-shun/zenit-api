using MediatR;

using Accounts.Business.Services;
using Accounts.Contract.Requests;


namespace Accounts.Host.RequestHandlers
{
    public class AccountUpdateHandler(AccountService accountService) : IRequestHandler<AccountUpdateRequest, AccountUpdateResponse>
    {
        public async Task<AccountUpdateResponse> Handle(AccountUpdateRequest request, CancellationToken cancellationToken)
        {
            return await accountService.Update(request);
        }
    }
}
using MediatR;

using Zenit.Management.Business.Services;
using Zenit.Management.Contract.Requests.AccountRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class AccountUpdateHandler(AccountService accountService) : IRequestHandler<AccountUpdateRequest, AccountUpdateResponse>
    {
        public async Task<AccountUpdateResponse> Handle(AccountUpdateRequest request, CancellationToken cancellationToken)
        {
            return await accountService.Update(request);
        }
    }
}

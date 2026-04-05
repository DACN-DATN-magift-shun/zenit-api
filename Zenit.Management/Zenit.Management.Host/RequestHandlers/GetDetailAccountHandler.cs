using MediatR;

using Zenit.Management.Business.Services.Accounts;
using Zenit.Management.Contract.Requests.AccountRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class AccountGetDetailHandler(AccountService accountService) : IRequestHandler<AccountGetDetailRequest, AccountGetDetailResponse>
    {
        public async Task<AccountGetDetailResponse> Handle(AccountGetDetailRequest request, CancellationToken cancellationToken)
        {
            return await accountService.GetDetail(request);
        }
    }
}

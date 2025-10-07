using Zenit.Accounts.Business.Services;
using Zenit.Accounts.Contract.Requests;
using MediatR;

namespace Zenit.Accounts.Host.RequestHandlers
{
    public class AccountGetDetailHandler(AccountService accountService) : IRequestHandler<AccountGetDetailRequest, AccountGetDetailResponse>
    {
        public async Task<AccountGetDetailResponse> Handle(AccountGetDetailRequest request, CancellationToken cancellationToken)
        {
            return await accountService.GetDetail(request);
        }
    }
}

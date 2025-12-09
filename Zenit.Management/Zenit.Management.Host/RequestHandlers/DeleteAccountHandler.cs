using MediatR;

using Zenit.Management.Business.Services;
using Zenit.Management.Contract.Requests.AccountRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class AccountDeleteHandler(AccountService accountService) : IRequestHandler<AccountDeleteRequest>
    {
        public async Task Handle(AccountDeleteRequest request, CancellationToken cancellationToken)
        {
            await accountService.Delete(request);
        }
    }
}

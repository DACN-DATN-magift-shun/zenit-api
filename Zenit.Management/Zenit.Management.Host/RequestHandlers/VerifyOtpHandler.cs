using MediatR;

using Zenit.Management.Business.Services;
using Zenit.Management.Contract.Requests.AccountRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class VerifyOtpHandler(AccountService accountService) : IRequestHandler<AccountVerifyOTPRequest, AccountVerifyOTPResponse>
    {
        public async Task<AccountVerifyOTPResponse> Handle(AccountVerifyOTPRequest request, CancellationToken cancellationToken)
        {
            return await accountService.VerifyOTP(request);
        }
    }
}
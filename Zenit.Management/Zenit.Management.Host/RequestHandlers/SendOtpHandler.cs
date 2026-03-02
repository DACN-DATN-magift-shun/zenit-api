using MediatR;

using Zenit.Management.Business.Services;
using Zenit.Management.Contract.Requests.AccountRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class SendOtpHandler(AccountService accountService) : IRequestHandler<AccountSendOTPRequest, AccountSendOTPResponse>
    {
        public async Task<AccountSendOTPResponse> Handle(AccountSendOTPRequest request, CancellationToken cancellationToken)
        {
            return await accountService.SendOTP(request);
        }
    }
}
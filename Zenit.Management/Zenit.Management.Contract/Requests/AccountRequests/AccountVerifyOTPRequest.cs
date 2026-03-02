using MediatR;

namespace Zenit.Management.Contract.Requests.AccountRequests
{
    public class AccountVerifyOTPRequest : IRequest<AccountVerifyOTPResponse>
    {
        public required string Email { get; set; }
        public required string OTP { get; set; }
    }

    public class AccountVerifyOTPResponse
    {
        public required string ResetToken { get; set; }
    }
}
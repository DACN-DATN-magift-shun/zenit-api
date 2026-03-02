using MediatR;

namespace Zenit.Management.Contract.Requests.AccountRequests
{
    public class AccountResetPasswordRequest : IRequest<AccountResetPasswordResponse>
    {
        public required string ResetToken { get; set; }
        public required string NewPassword { get; set; }
    }

    public class AccountResetPasswordResponse
    {
        public required int StatusCode { get; set; }
        public required string Message { get; set; }
    }
}
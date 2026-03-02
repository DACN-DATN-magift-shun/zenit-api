
using MediatR;

using SendGrid;

namespace Zenit.Management.Contract.Requests.AccountRequests
{
    public class AccountSendOTPRequest : IRequest<AccountSendOTPResponse>
    {
        public required string Email { get; set; }
    }

    public class AccountSendOTPResponse
    {
        public object? SendEmailResponse { get; set; }
    }
}
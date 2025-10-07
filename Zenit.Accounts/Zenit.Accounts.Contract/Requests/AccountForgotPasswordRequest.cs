using MediatR;


namespace Zenit.Accounts.Contract.Requests
{
    public class AccountForgotPasswordRequest : IRequest<AccountForgotPasswordResponse>
    {
        public required string Email { get; set; }
    }

    public class AccountForgotPasswordResponse
    {
        public required string Message { get; set; }
        public required string ResetToken { get; set; }
        public required DateTime Expiration { get; set; }
        
    }
}

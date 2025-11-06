using MediatR;


namespace Zenit.Accounts.Contract.Requests
{
    public class AccountLoginRequest : IRequest<AccountLoginResponse>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class AccountLoginResponse
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}

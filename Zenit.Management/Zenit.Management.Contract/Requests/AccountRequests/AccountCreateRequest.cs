using MediatR;


namespace Zenit.Management.Contract.Requests.AccountRequests
{
    public class AccountCreateRequest : IRequest<AccountCreateResponse>
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public required string Password { get; set; }
    }

    public class AccountCreateResponse
    {
        public required string Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}

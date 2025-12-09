using MediatR;

namespace Zenit.Management.Contract.Requests.AccountRequests
{
    public class AccountGetDetailRequest : IRequest<AccountGetDetailResponse>
    {
        // No properties needed as we will get the user info from the authentication context
    }

    public class AccountGetDetailResponse
    {
        public required string Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}

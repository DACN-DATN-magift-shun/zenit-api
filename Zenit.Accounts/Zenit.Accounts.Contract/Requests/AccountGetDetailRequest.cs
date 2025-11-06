using MediatR;

using Microsoft.AspNetCore.Mvc;


namespace Zenit.Accounts.Contract.Requests
{
    public class AccountGetDetailRequest : IRequest<AccountGetDetailResponse>
    {
        [FromRoute]
        public required string Id { get; set; }
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

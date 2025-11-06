using MediatR;


namespace Zenit.Accounts.Contract.Requests
{
    public class AccountUpdateRequest : IRequest<AccountUpdateResponse>
    {
        public required string Id { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }

    public class AccountUpdateResponse
    {
        public required string Id { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}

using MediatR;


namespace Zenit.Management.Contract.Requests.AccountRequests
{
    public class AccountUpdateRequest : IRequest<AccountUpdateResponse>
    {
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

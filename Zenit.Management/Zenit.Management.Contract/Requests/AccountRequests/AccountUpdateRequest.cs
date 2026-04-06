using MediatR;

using Zenit.Management.Data.Entities;


namespace Zenit.Management.Contract.Requests.AccountRequests
{
    public class AccountUpdateRequest : IRequest<AccountUpdateResponse>
    {
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public Guid? PhotoId { get; set; }
    }

    public class AccountUpdateResponse
    {
        public required string Id { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public Guid? PhotoId { get; set; }
        public virtual Photo? Photo { get; set; }
    }
}

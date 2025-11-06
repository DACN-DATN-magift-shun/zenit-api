using MediatR;


namespace Zenit.Accounts.Contract.Requests
{
    public class AccountDeleteRequest : IRequest
    {
        public required string Id { get; set; }
    }
}

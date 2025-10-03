using MediatR;


namespace Accounts.Contract.Requests
{
    public class AccountDeleteRequest : IRequest
    {
        public required string Id { get; set; }
    }
}
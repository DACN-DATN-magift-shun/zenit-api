using MediatR;


namespace Zenit.Management.Contract.Requests.AccountRequests
{
    public class AccountIsEmailExistRequest : IRequest<AccountIsEmailExistResponse>
    {
        public required string Email { get; set; }
    }

    public class AccountIsEmailExistResponse
    {
        public required bool Result { get; set; }
    }
}

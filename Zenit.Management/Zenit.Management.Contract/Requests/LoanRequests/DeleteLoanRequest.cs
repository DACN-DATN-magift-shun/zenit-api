using MediatR;

namespace Zenit.Management.Contract.Requests.LoanRequests
{
    public class DeleteLoanRequest : IRequest
    {
        public required Guid Id { get; set; }
    }
}
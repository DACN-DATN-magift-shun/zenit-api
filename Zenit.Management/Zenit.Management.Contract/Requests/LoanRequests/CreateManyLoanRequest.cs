using MediatR;

namespace Zenit.Management.Contract.Requests.LoanRequests
{
    public class CreateManyLoanRequest : IRequest<CreateManyLoanResponse>
    {
        public required List<CreateLoanRequest> Loans { get; set; }
    }

    public class CreateManyLoanResponse
    {
        public required List<CreateLoanResponse> Loans { get; set; }
    }
}
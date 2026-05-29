using MediatR;

using Zenit.Management.Common.Enums;


namespace Zenit.Management.Contract.Requests.LoanRequests
{
    public class GetDetailLoanRequest : IRequest<GetDetailLoanResponse>
    {
        public required Guid Id { get; set; }
    }

    public class GetDetailLoanResponse
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required LoanType Type { get; set; }
        public required int Amount { get; set; }
        public required DateTime Date { get; set; }
        public required DateTime DueDate { get; set; }
        public required LoanStatus Status { get; set; }
        public string? Note { get; set; }
    }
}

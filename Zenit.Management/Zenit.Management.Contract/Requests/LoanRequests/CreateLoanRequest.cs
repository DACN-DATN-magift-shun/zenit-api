using MediatR;

using Zenit.Management.Common.Enums;


namespace Zenit.Management.Contract.Requests.LoanRequests
{
    public class CreateLoanRequest : IRequest<CreateLoanResponse>
    {
        public required string Name { get; set; }
        public required LoanType Type { get; set; }
        public required int Amount { get; set; }
        public required DateTime Date { get; set; }
        public required DateTime DueDate { get; set; }
        public string? Note { get; set; }
    }

    public class CreateLoanResponse
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required LoanType Type { get; set; }
        public required int Amount { get; set; }
        public required DateTime Date { get; set; }
        public required DateTime DueDate { get; set; }
        public string? Note { get; set; }
    }
}

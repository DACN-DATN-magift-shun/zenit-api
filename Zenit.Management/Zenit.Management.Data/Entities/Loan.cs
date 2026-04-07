using Zenit.Management.Common.Enums;
using Zenit.Management.Data.Models;

namespace Zenit.Management.Data.Entities
{
    public class Loan : ManagementAuditModel
    {
        public required string Name { get; set; }
        public required LoanType Type { get; set; }
        public required int Amount { get; set; }
        public required DateTime Date { get; set; }
        public required DateTime DueDate { get; set; }
        public string? Note { get; set; }
        public required LoanStatus Status { get; set; }
        public required Guid AccountId { get; set; }
    }
}
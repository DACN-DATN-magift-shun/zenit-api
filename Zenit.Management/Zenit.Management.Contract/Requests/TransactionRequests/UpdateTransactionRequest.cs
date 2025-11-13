using MediatR;

namespace Zenit.Management.Contract.TransactionRequests
{
    public class UpdateTransactionRequest : IRequest<UpdateTransactionResponse>
    {
        public required Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Note { get; set; }
        public int? Amount { get; set; }
        public DateTime? TransactionDate { get; set; }
        public Guid? CategoryId { get; set; }
    }

    public class UpdateTransactionResponse
    {
        public required Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Note { get; set; }
        public required int Amount { get; set; }
        public required DateTime TransactionDate { get; set; }
        public required Guid CategoryId { get; set; }
    }
}
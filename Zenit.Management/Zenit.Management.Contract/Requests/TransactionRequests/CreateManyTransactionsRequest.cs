using MediatR;

namespace Zenit.Management.Contract.TransactionRequests
{
    public class CreateManyTransactionsRequest : IRequest<CreateManyTransactionsResponse>
    {
        public required List<TransactionCreateFields> Transactions { get; set; }
    }

    public class CreateManyTransactionsResponse
    {
        public required List<TransactionResponse> Transactions { get; set; }
    }

    public class TransactionCreateFields
    {
        public required string Title { get; set; }
        public string? Note { get; set; }
        public required int Amount { get; set; }
        public required DateTime TransactionDate { get; set; }
        public required Guid CategoryId { get; set; }
    }

    public class TransactionResponse
    {
        public required Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Note { get; set; }
        public required int Amount { get; set; }
        public required DateTime TransactionDate { get; set; }
        public required Guid CategoryId { get; set; }
    }
}
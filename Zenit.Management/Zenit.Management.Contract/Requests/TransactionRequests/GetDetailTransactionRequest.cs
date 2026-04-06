using MediatR;

using Zenit.Management.Data.Entities;

namespace Zenit.Management.Contract.TransactionRequests
{
    public class GetDetailTransactionRequest : IRequest<GetDetailTransactionResponse>
    {
        public required Guid Id { get; set; }
    }

    public class GetDetailTransactionResponse
    {
        public required string Title { get; set; }
        public string? Note { get; set; }
        public required int Amount { get; set; }
        public required DateTime TransactionDate { get; set; }
        public required Guid WalletId { get; set; }
        public required Guid CategoryId { get; set; }
        public virtual Wallet? Wallet { get; set; }
        public virtual Category? Category { get; set; }
    }
}
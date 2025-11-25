using MediatR;

namespace Zenit.Management.Contract.TransactionRequests
{
    public class CreateManyTransactionsRequest : IRequest<CreateManyTransactionsResponse>
    {
        public required List<CreateTransactionRequest> Transactions { get; set; }
    }

    public class CreateManyTransactionsResponse
    {
        public required List<CreateTransactionResponse> Transactions { get; set; }
    }
}
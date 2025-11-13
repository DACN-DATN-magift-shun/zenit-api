using MediatR;

namespace Zenit.Management.Contract.TransactionRequests
{
    public class UpdateManyTransactionsRequest : IRequest<UpdateManyTransactionsResponse>
    {
        public required List<UpdateTransactionRequest> Transactions { get; set; }
    }

    public class UpdateManyTransactionsResponse
    {
        public required List<TransactionResponse> Transactions { get; set; }
    }
}
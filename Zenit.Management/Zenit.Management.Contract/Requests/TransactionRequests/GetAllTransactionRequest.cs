using System.Transactions;

using MediatR;

namespace Zenit.Management.Contract.TransactionRequests
{
    public class GetAllTransactionRequest : IRequest<GetAllTransactionResponse>
    {
    }

    public class GetAllTransactionResponse
    {
        List<Transaction>? Transactions { get; set; }
    }
}
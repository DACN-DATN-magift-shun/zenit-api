using MediatR;

using Zenit.Management.Data.Entities;

namespace Zenit.Management.Contract.TransactionRequests
{
    public class GetAllTransactionRequest : IRequest<GetAllTransactionResponse>
    {
    }

    public class GetAllTransactionResponse
    {
        public List<Transaction>? Transactions { get; set; }
    }
}
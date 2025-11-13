using MediatR;

namespace Zenit.Management.Contract.TransactionRequests
{
    public class DeleteTransactionRequest : IRequest
    {
        public required Guid Id { get; set; }
    }
}
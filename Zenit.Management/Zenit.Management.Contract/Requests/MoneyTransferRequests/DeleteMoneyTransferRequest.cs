using MediatR;

namespace Zenit.Management.Contract.Request.MoneyTransferRequests
{
    public class DeleteMoneyTransferRequest : IRequest
    {
        public required Guid Id { get; set; }
    }

    public class DeleteMoneyTransferResponse
    {
    }
}
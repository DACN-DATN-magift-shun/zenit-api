using MediatR;

using Zenit.Management.Data.Entities;

namespace Zenit.Management.Contract.Request.MoneyTransferRequests
{
    public class GetDetailMoneyTransferRequest : IRequest<GetDetailMoneyTransferResponse>
    {
        public required Guid Id { get; set; }
    }

    public class GetDetailMoneyTransferResponse
    {
        public required Guid FromWalletId { get; set; }
        public required Guid ToWalletId { get; set; }
        public required int Amount { get; set; }
        public required DateTime TransferDate { get; set; }
        public string? Note { get; set; }
        public virtual Wallet? FromWallet { get; set; }
        public virtual Wallet? ToWallet { get; set; }
    }
}
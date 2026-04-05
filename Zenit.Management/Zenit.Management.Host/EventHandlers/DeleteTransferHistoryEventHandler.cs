using Zenit.Management.Business.Services.WalletServices;
using Zenit.Management.Data.Entities;
using Zenit.Share.Common.Values;
using Zenit.Share.Data.Events.Handlers;

namespace Zenit.Management.Host.EventHandlers
{
    public class DeleteTransferHistoryEventHandler(WalletService walletService) : EventDeleteHandler<MoneyTransferHistory>
    {
        public override async Task Handle(MoneyTransferHistory data, List<AuditDataChange>? dataChanges)
        {
            await walletService.HandleDeleteMoneyTransferHistoryAsync(data, dataChanges);
        }
    }
}
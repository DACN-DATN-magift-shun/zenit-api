using Zenit.Management.Business.Services.WalletServices;
using Zenit.Management.Data.Entities;
using Zenit.Share.Common.Values;
using Zenit.Share.Data.Events.Handlers;

namespace Zenit.Management.Host.EventHandlers
{
    public class UpdateTransferHistoryEventHandler(WalletService walletService) : EventUpdateHandler<MoneyTransferHistory>
    {
        public override async Task Handle(MoneyTransferHistory data, List<AuditDataChange>? dataChanges)
        {
            await walletService.HandleUpdateMoneyTransferHistoryAsync(data, dataChanges);
        }
    }
}
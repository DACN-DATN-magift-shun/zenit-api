using Zenit.Management.Business.Services.WalletServices;
using Zenit.Management.Data.Entities;
using Zenit.Share.Common.Values;
using Zenit.Share.Data.Events.Handlers;

namespace Zenit.Management.Host.EventHandlers
{
    public class CreateTransferHistoryEventHandler(WalletService walletService) : EventCreateHandler<MoneyTransferHistory>
    {
        public override async Task Handle(MoneyTransferHistory data, List<AuditDataChange>? dataChanges)
        {
            await walletService.HandleCreateMoneyTransferHistoryAsync(data, dataChanges);
        }
    }
}
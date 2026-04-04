using Zenit.Management.Business.Services;
using Zenit.Management.Data.Entities;
using Zenit.Share.Common.Values;
using Zenit.Share.Data.Events.Handlers;

namespace Zenit.Management.Host.EventHandlers
{
    public class CreateWalletEventHandler(MoneyTransferService moneyTransferService) : EventCreateHandler<Wallet>
    {
        public override async Task Handle(Wallet data, List<AuditDataChange>? dataChanges)
        {
            await moneyTransferService.HandleCreateWalletAsync(data, dataChanges);
        }
    }
}
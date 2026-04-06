using MediatR;

using Zenit.Management.Business.Services;
using Zenit.Management.Data.Entities;
using Zenit.Share.Common.Values;
using Zenit.Share.Data.Events.Handlers;

namespace Zenit.Management.Host.EventHandlers
{
    public class CreateTransactionEventHandler(StatisticsService statisticsService, WalletService walletService) : EventCreateHandler<Transaction>
    {
        public override async Task Handle(Transaction data, List<AuditDataChange>? dataChanges)
        {
            await statisticsService.HandleCreateTransactionAsync(data, dataChanges);
            await walletService.HandleCreateTransactionAsync(data, dataChanges);
        }
    }
}
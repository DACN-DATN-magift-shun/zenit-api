using MediatR;

using Zenit.Management.Business.Services;
using Zenit.Management.Data.Entities;
using Zenit.Share.Common.Values;
using Zenit.Share.Data.Events.Handlers;

namespace Zenit.Management.Host.EventHandlers
{
    public class UpdateTransactionEventHandler(StatisticsService statisticsService) : EventUpdateHandler<Transaction>
    {
        public override async Task Handle(Transaction data, List<AuditDataChange>? dataChanges)
        {
            await statisticsService.HandleUpdateTransactionAsync(data, dataChanges);
        }
    }
}
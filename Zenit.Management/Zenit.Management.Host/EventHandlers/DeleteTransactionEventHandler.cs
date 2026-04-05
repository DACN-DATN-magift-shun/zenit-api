using MediatR;

using Zenit.Management.Business.Services.StatisticsServices;
using Zenit.Management.Data.Entities;
using Zenit.Share.Common.Values;
using Zenit.Share.Data.Events.Handlers;

namespace Zenit.Management.Host.EventHandlers
{
    public class DeleteTransactionEventHandler(StatisticsService statisticsService) : EventDeleteHandler<Transaction>
    {
        public override async Task Handle(Transaction data, List<AuditDataChange>? dataChanges)
        {
            await statisticsService.HandleDeleteTransactionAsync(data, dataChanges);
        }
    }
}
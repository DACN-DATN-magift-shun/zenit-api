using MediatR;

using Zenit.Share.Common.Interfaces;
using Zenit.Share.Common.Values;
using Zenit.Share.Data.Events.Requests;
using Zenit.Share.Data.Interfaces;

namespace Zenit.Share.Data.Events.Handlers
{
    public abstract class EventHandler<T, TNotification> : INotificationHandler<TNotification>
        where T : class
        where TNotification : EventRequest<IDataModel>
    {
        public async Task Handle(TNotification notification, CancellationToken cancellationToken)
        {
            if (notification.Data is T data)
            {
                await Handle(data, notification.DataChanges);
            }
        }

        public abstract Task Handle(T data, List<AuditDataChange>? dataChanges);
    }
}
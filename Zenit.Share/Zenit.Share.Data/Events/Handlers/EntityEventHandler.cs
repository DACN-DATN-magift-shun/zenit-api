using Zenit.Share.Common;
using Zenit.Share.Common.Values;
using Zenit.Share.Data.Interfaces;

namespace Zenit.Share.Data.Events.Handlers
{
    public abstract class EntityEventHandler<T, TNotification> : IEventHandler
        where TNotification : EventRequest<IDataModel>
    {
        public async Task Handle(TNotification notification, CancellationToken cancellationToken)
        {
            if (notification.Data is T data)
            {
                try
                {
                    await Handle(data, notification.FieldChanges, cancellationToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }   

        protected abstract Task Handle(T entity, List<AuditFieldChange>? fieldChanges, CancellationToken cancellationToken);
    }
}
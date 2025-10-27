using MediatR;

using Zenit.Share.Data.Events.Requests;
using Zenit.Share.Data.Interfaces;

namespace Zenit.Share.Data.Events.Handlers
{
    public abstract class EntityDeletionEventHandler<T> : EntityEventHandler<T, EntityDeletionEventRequest<IDataModel>>, INotificationHandler<EntityDeletionEventRequest<IDataModel>>
    {
    }
}
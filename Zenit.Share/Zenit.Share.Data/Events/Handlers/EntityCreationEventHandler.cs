using MediatR;

using Zenit.Share.Data.Events.Requests;
using Zenit.Share.Data.Interfaces;

namespace Zenit.Share.Data.Events.Handlers
{
    public abstract class EntityCreationEventHandler<T> : EntityEventHandler<T, EntityCreationEventRequest<IDataModel>>, INotificationHandler<EntityCreationEventRequest<IDataModel>>
    {
    }
}
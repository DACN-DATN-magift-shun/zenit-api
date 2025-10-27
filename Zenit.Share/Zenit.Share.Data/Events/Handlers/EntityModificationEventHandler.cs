using MediatR;

using Zenit.Share.Data.Events.Requests;
using Zenit.Share.Data.Interfaces;

namespace Zenit.Share.Data.Events.Handlers
{
    public abstract class EntityModificationEventHandler<T> : EntityEventHandler<T, EntityModificationEventRequest<IDataModel>>, INotificationHandler<EntityModificationEventRequest<IDataModel>>
    {
    }
}
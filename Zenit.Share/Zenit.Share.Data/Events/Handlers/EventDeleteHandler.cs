using Zenit.Share.Common.Values;
using Zenit.Share.Data.Events.Requests;
using Zenit.Share.Data.Interfaces;

namespace Zenit.Share.Data.Events.Handlers
{
    public abstract class EventDeleteHandler<T> : EventHandler<T, EventDeleteRequest<IDataModel>>
        where T : class
    {
    }
}
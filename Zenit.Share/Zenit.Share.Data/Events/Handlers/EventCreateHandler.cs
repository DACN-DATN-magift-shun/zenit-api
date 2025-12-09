using Zenit.Share.Common.Values;
using Zenit.Share.Data.Events.Requests;
using Zenit.Share.Data.Interfaces;

namespace Zenit.Share.Data.Events.Handlers
{
    public abstract class EventCreateHandler<T> : EventHandler<T, EventCreateRequest<IDataModel>>
        where T : class
    {
    }
}
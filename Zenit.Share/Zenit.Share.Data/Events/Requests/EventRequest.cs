using Zenit.Share.Common.Interfaces;
using Zenit.Share.Common.Values;

namespace Zenit.Share.Data.Events.Requests
{
    public abstract class EventRequest<T> : IEventRequest<T>
    {
        public required T Data { get; set; }
        public List<AuditDataChange>? DataChanges { get; set; }
    }
}
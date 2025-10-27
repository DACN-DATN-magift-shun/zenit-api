using Zenit.Share.Common.Interfaces;
using Zenit.Share.Common.Values;

namespace Zenit.Share.Common
{
    public abstract class EventRequest<T> : IEventRequest<T>
    {
        public T Data { get; set; }
        public List<AuditFieldChange>? FieldChanges { get; set; }
    }
}
using MediatR;

using Zenit.Share.Common.Values;

namespace Zenit.Share.Common.Interfaces
{
    public interface IEventRequest : INotification
    {

    }

    public interface IEventRequest<T> : IEventRequest
    {
        T Data { get; set; }
        List<AuditFieldChange>? FieldChanges { get; set; }
    }
}
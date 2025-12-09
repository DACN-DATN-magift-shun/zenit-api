using MediatR;

namespace Zenit.Share.Common.Interfaces
{
    public interface IEventRequest : INotification
    {
    }

    public interface IEventRequest<T> : IEventRequest
    {
        public T Data { get; set; }
    }
}
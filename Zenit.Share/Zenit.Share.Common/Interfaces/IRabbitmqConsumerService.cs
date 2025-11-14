using RabbitMQ.Client.Events;

namespace Zenit.Share.Common.Interfaces
{
    public interface IRabbitmqConsumer
    {
        Task ConsumeMessageAsync(AsyncEventHandler<BasicDeliverEventArgs> callback);
    }
}
using RabbitMQ.Client.Events;

using Zenit.Share.Common.Services;

namespace Zenit.Share.Common.Interfaces
{
    public interface IRabbitmqConsumer
    {
        Task ConsumeMessageAsync(AsyncEventHandler<BasicDeliverEventArgs> callback, RabbitmqConsumerRequest request);
    }
}
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using Zenit.Share.Common.Interfaces;

namespace Zenit.Share.Common.Services
{
    public class RabbitmqConsumerRequest
    {
        public required string Exchange { get; set; }
        public required string Queue { get; set; }
        public required string RoutingKey { get; set; }
        public string ExchangeType { get; set; } = "direct";
    }

    public class RabbitmqConsumerService(IConnection connection, RabbitmqConsumerRequest request) : IRabbitmqConsumer
    {
        public async Task<IChannel> InititalizeConsumerAsync()
        {
            using var _channel = await connection.CreateChannelAsync();
            await _channel.ExchangeDeclareAsync(
                exchange: request.Exchange,
                type: request.ExchangeType
            );

            await _channel.QueueDeclareAsync(
                queue: request.Queue,
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            await _channel.QueueBindAsync(
                queue: request.Queue,
                exchange: request.Exchange,
                routingKey: request.RoutingKey
            );

            return _channel;
        }

        public async Task ConsumeMessageAsync(
            AsyncEventHandler<BasicDeliverEventArgs> callback)
        {
            using var _channel = await InititalizeConsumerAsync();
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += callback;

            await _channel.BasicConsumeAsync(
                queue: request.Queue,
                autoAck: false,
                consumer: consumer
            );
        }
    }
}
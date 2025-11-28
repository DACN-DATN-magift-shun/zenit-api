using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using Zenit.Share.Common.Interfaces;

namespace Zenit.Share.Common.Services
{
    public class RabbitmqConsumerRequest
    {
        public required string Exchange { get; set; }
        public string? Queue { get; set; }
        public required string RoutingKey { get; set; }
        public required string ExchangeType { get; set; }
    }

    public class RabbitmqConsumerService(IConnection connection) : IRabbitmqConsumer
    {

        public async Task ConsumeMessageAsync(
            AsyncEventHandler<BasicDeliverEventArgs> callback, RabbitmqConsumerRequest request)
        {
            var channel = await InititalizeConsumerAsync(request);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (sender, ea) =>
            {
                try
                {
                    // Invoke callback
                    await callback(sender, ea);

                    // Acknowledge message
                    await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");

                    // Nack message để có thể retry
                    await channel.BasicNackAsync(
                        deliveryTag: ea.DeliveryTag,
                        multiple: false,
                        requeue: true
                    );
                }
                // ✅ XÓA dòng channel.CloseAsync() - để channel tiếp tục nhận messages
            };

            await channel.BasicConsumeAsync(
                queue: request.Queue ?? "",
                autoAck: false,
                consumer: consumer
            );

            channel.ChannelShutdownAsync += async (sender, ea) =>
            {
                Console.WriteLine($"Channel shutdown: {ea.ReplyText}");
                await Task.CompletedTask;
            };
        }

        public async Task<IChannel> InititalizeConsumerAsync(RabbitmqConsumerRequest request)
        {
            var _channel = await connection.CreateChannelAsync();
            await _channel.ExchangeDeclareAsync(
                exchange: request.Exchange,
                type: request.ExchangeType
            );

            await _channel.QueueDeclareAsync(
                queue: request.Queue ?? "",
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            await _channel.QueueBindAsync(
                queue: request.Queue ?? "",
                exchange: request.Exchange,
                routingKey: request.RoutingKey
            );

            return _channel;
        }
    }
}
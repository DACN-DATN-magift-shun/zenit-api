using System.Text;

using RabbitMQ.Client;

using Zenit.Share.Common.Interfaces;

namespace Zenit.Share.Common.Services
{
    public class RabbitmqProducerRequest
    {
        public required string Exchange { get; set; }
        public required string RoutingKey { get; set; }
        public required string Body { get; set; }
        public string ExchangeType { get; set; } = "direct";
    }

    public class RabbitmqProducerService(IConnection connection, RabbitmqProducerRequest request) : IRabbitmqProducer
    {
        public async Task PublishMessageAsync()
        {
            using var _channel =  await connection.CreateChannelAsync();

            await _channel.ExchangeDeclareAsync(
                exchange: request.Exchange,
                type: request.ExchangeType
            );

            await _channel.BasicPublishAsync(
                exchange: request.Exchange,
                routingKey: request.RoutingKey,
                body: Encoding.UTF8.GetBytes(request.Body)
            );
        }
    }
}
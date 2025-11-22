using System.Text;

using RabbitMQ.Client;

using Zenit.Share.Common.Interfaces;

namespace Zenit.Share.Common.Services
{
    public class RabbitmqProducerRequest
    {
        public required string Exchange { get; set; }
        public required string RoutingKey { get; set; }
        public string? Body { get; set; }
        public required string ExchangeType { get; set; }
    }

    public class RabbitmqProducerService(IConnection connection) : IRabbitmqProducer
    {
        // IChannel? _channel;

        public async Task PublishMessageAsync(RabbitmqProducerRequest request)
        {
            using var _channel =  await connection.CreateChannelAsync();

            await _channel.ExchangeDeclareAsync(
                exchange: request.Exchange,
                type: request.ExchangeType
            );


            await _channel.BasicPublishAsync(
                exchange: request.Exchange,
                routingKey: request.RoutingKey,
                mandatory: true,
                body: request.Body != "" ? Encoding.UTF8.GetBytes(request.Body) : Array.Empty<byte>()
            );
        }
    }
}
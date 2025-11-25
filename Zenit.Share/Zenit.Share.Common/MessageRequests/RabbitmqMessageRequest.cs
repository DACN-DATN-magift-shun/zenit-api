using Zenit.Share.Common.Interfaces;

namespace Zenit.Share.Common.MessageRequests
{
    public class RabbitmqMessageRequest : IRabbitmqMessageRequest
    {
        public required string Exchange { get; set; }
        public required string RoutingKey { get; set; }
        public required string ExchangeType { get; set; }
        public string? Body { get; set; }
    }
}
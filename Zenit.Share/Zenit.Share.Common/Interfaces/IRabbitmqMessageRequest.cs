using MediatR;

namespace Zenit.Share.Common.Interfaces
{
    public interface IRabbitmqMessageRequest : INotification
    {
        public string Exchange { get; set; }
        public string RoutingKey { get; set; }
        public string ExchangeType { get; set; }
        public string? Body { get; set; }
    }
}
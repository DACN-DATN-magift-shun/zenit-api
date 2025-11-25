using Zenit.Share.Common.Services;

namespace Zenit.Share.Common.Interfaces
{
    public interface IRabbitmqProducer
    {
        Task PublishMessageAsync(RabbitmqProducerRequest request);
    }
}
using MediatR;

namespace Zenit.Management.Contract.Requests.MessageRequests
{
    public class StreamMessageRequest : IRequest<StreamMessageResponse>
    {
        public required Guid ConversationId { get; set; }
    }

    public class StreamMessageResponse
    {
        public required Guid ConversationId { get; set; }
    }
}
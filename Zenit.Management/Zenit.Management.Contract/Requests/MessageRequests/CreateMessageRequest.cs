using MediatR;

namespace Zenit.Management.Contract.Requests.MessageRequests
{
    public class CreateMessageRequest : IRequest<CreateMessageResponse>
    {
        public required Guid ConversationId { get; set; }
        public required string Text { get; set; }
    }

    public class CreateMessageResponse
    {
        public required Guid Id { get; set; }
        public required Guid ConversationId { get; set; }
        public required string Text { get; set; }
    }
}
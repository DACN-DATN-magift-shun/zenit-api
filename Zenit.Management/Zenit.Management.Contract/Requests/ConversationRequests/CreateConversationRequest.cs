using MediatR;

namespace Zenit.Management.Contract.Requests.ConversationRequests
{
    public class CreateConversationRequest : IRequest<CreateConversationResponse>
    {
        public required string Title { get; set; }
    }

    public class CreateConversationResponse
    {
        public required Guid Id { get; set; }
        public required string Title { get; set; }
    }
}
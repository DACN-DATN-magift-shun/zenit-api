using MediatR;

namespace Zenit.Management.Contract.Requests.ConversationRequests
{
    public class UpdateConversationRequest : IRequest<UpdateConversationResponse>
    {
        public required Guid Id { get; set; }
        public required string Title { get; set; }
    }

    public class UpdateConversationResponse
    {
        public required Guid Id { get; set; }
        public required string Title { get; set; }
    }
}
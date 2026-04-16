using MediatR;

namespace Zenit.Management.Contract.Requests.ConversationRequests
{
    public class GetDetailConversationRequest : IRequest<GetDetailConversationResponse>
    {
        public required Guid Id { get; set; }
    }

    public class GetDetailConversationResponse
    {
        public required Guid Id { get; set; }
        public required string Title { get; set; }
    }
}
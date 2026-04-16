using MediatR;

namespace Zenit.Management.Contract.Requests.ConversationRequests
{
    public class DeleteConversationRequest : IRequest
    {
        public required Guid Id { get; set; }
    }

    public class DeleteConversationResponse
    {
    }
}
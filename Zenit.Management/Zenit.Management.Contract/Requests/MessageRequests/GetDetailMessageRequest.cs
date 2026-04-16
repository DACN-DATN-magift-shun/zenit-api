using MediatR;

namespace Zenit.Management.Contract.Requests.MessageRequests
{
    public class GetDetailMessageRequest : IRequest<GetDetailMessageResponse>
    {
        public required Guid Id { get; set; }
    }

    public class GetDetailMessageResponse
    {
        public required Guid Id { get; set; }
        public required Guid AccountId { get; set; }
        public required Guid ConversationId { get; set; }
        public required string Text { get; set; }
    }
}
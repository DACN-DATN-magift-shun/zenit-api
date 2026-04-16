using MediatR;

namespace Zenit.Management.Contract.Requests.MessageRequests
{
    public class UpdateMessageRequest : IRequest<UpdateMessageResponse>
    {
        public required Guid Id { get; set; }
        public required string Text { get; set; }
    }

    public class UpdateMessageResponse
    {
        public required Guid Id { get; set; }
        public required string Text { get; set; }
    }
}
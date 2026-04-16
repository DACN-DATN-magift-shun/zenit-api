using MediatR;

namespace Zenit.Management.Contract.Requests.MessageRequests
{
    public class DeleteMessageRequest : IRequest
    {
        public required Guid Id { get; set; }
    }

    public class DeleteMessageResponse
    {
    }
}
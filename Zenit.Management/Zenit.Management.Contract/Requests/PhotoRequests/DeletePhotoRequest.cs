using MediatR;

namespace Zenit.Management.Contract.Requests.PhotoRequests
{
    public class DeletePhotoRequest : IRequest
    {
        public required Guid Id { get; set; }
    }

    public class DeletePhotoResponse
    {
    }
}
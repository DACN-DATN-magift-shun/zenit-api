using MediatR;

using Zenit.Share.Contract.Models;

namespace Zenit.Management.Contract.Requests.PhotoRequests
{
    public class GetAllPhotoRequest : ScrollPaginationRequest, IRequest<GetAllPhotoResponse>
    {
        public required Guid TransactionId { get; set; }
    }

    public class GetAllPhotoResponse : PaginationResponse<GetAllPhotoResponseItem>
    {
    }

    public class GetAllPhotoResponseItem : GetDetailPhotoResponse
    {
    }
}
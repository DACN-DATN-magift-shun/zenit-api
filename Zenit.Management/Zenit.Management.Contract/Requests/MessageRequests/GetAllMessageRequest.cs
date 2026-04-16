using MediatR;

using Zenit.Share.Contract.Models;

namespace Zenit.Management.Contract.Requests.MessageRequests
{
    public class GetAllMessageRequest : ScrollPaginationRequest, IRequest<GetAllMessageResponse>
    {
        public Guid ConversationId { get; set; }
    }

    public class GetAllMessageResponse : PaginationResponse<GetAllMessageResponseItem>
    {
    }
    
    public class GetAllMessageResponseItem : GetDetailMessageResponse
    {
    }
}
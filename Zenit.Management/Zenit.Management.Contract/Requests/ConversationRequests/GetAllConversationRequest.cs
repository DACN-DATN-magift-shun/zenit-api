using MediatR;

using Zenit.Share.Contract.Models;

namespace Zenit.Management.Contract.Requests.ConversationRequests
{
    public class GetAllConversationRequest : ScrollPaginationRequest, IRequest<GetAllConversationResponse>
    {
    }

    public class GetAllConversationResponse : PaginationResponse<GetAllConversationResponseItem>
    {
    }
    
    public class GetAllConversationResponseItem : GetDetailConversationResponse
    {
    }
}
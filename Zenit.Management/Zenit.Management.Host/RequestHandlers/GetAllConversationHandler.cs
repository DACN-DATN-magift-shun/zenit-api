using MediatR;

using Zenit.Management.Business.Services.ConversationServices;
using Zenit.Management.Contract.Requests.ConversationRequests;


namespace Zenit.Management.Host.RequestHandlers
{
    public class GetAllConversationsHandler(ConversationService conversationService) : IRequestHandler<GetAllConversationRequest, GetAllConversationResponse>
    {
        public async Task<GetAllConversationResponse> Handle(GetAllConversationRequest request, CancellationToken cancellationToken)
        {
            return await conversationService.GetAll(request);
        }
    }
}
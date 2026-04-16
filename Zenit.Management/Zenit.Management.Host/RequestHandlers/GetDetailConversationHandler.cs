using MediatR;

using Zenit.Management.Business.Services.ConversationServices;
using Zenit.Management.Contract.Requests.ConversationRequests;


namespace Zenit.Management.Host.RequestHandlers
{
    public class GetDetailConversationHandler(ConversationService conversationService) : IRequestHandler<GetDetailConversationRequest, GetDetailConversationResponse>
    {
        public async Task<GetDetailConversationResponse> Handle(GetDetailConversationRequest request, CancellationToken cancellationToken)
        {
            return await conversationService.GetDetail(request);
        }
    }
}
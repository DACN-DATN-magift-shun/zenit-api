using MediatR;

using Zenit.Management.Business.Services.ConversationServices;
using Zenit.Management.Contract.Requests.ConversationRequests;


namespace Zenit.Management.Host.RequestHandlers
{
    public class UpdateConversationsHandler(ConversationService conversationService) : IRequestHandler<UpdateConversationRequest, UpdateConversationResponse>
    {
        public async Task<UpdateConversationResponse> Handle(UpdateConversationRequest request, CancellationToken cancellationToken)
        {
            return await conversationService.Update(request);
        }
    }
}
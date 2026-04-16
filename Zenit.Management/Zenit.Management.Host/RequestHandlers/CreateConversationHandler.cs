using MediatR;

using Zenit.Management.Business.Services.ConversationServices;
using Zenit.Management.Contract.Requests.ConversationRequests;


namespace Zenit.Management.Host.RequestHandlers
{
    public class CreateConversationHandler(ConversationService conversationService) : IRequestHandler<CreateConversationRequest, CreateConversationResponse>
    {
        public async Task<CreateConversationResponse> Handle(CreateConversationRequest request, CancellationToken cancellationToken)
        {
            return await conversationService.Create(request);
        }
    }
}
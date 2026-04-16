using MediatR;

using Zenit.Management.Business.Services.ConversationServices;
using Zenit.Management.Contract.Requests.ConversationRequests;


namespace Zenit.Management.Host.RequestHandlers
{
    public class DeleteConversationHandler(ConversationService conversationService) : IRequestHandler<DeleteConversationRequest>
    {
        public async Task Handle(DeleteConversationRequest request, CancellationToken cancellationToken)
        {
            await conversationService.Delete(request);
        }
    }
}
using MediatR;

using Zenit.Management.Business.Services.MessageServices;
using Zenit.Management.Contract.Requests.MessageRequests;


namespace Zenit.Management.Host.RequestHandlers
{
    public class UpdateMessagesHandler(MessageService messageService) : IRequestHandler<UpdateMessageRequest, UpdateMessageResponse>
    {
        public async Task<UpdateMessageResponse> Handle(UpdateMessageRequest request, CancellationToken cancellationToken)
        {
            return await messageService.Update(request);
        }
    }
}
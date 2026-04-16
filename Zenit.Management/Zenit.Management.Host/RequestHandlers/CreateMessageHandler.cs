using MediatR;

using Zenit.Management.Business.Services.MessageServices;
using Zenit.Management.Contract.Requests.MessageRequests;


namespace Zenit.Management.Host.RequestHandlers
{
    public class CreateMessageHandler(MessageService messageService) : IRequestHandler<CreateMessageRequest, CreateMessageResponse>
    {
        public async Task<CreateMessageResponse> Handle(CreateMessageRequest request, CancellationToken cancellationToken)
        {
            return await messageService.Create(request);
        }
    }
}
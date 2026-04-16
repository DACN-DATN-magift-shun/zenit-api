using MediatR;

using Zenit.Management.Business.Services.MessageServices;
using Zenit.Management.Contract.Requests.MessageRequests;


namespace Zenit.Management.Host.RequestHandlers
{
    public class DeleteMessageHandler(MessageService messageService) : IRequestHandler<DeleteMessageRequest>
    {
        public async Task Handle(DeleteMessageRequest request, CancellationToken cancellationToken)
        {
            await messageService.Delete(request);
        }
    }
}
using MediatR;

using Zenit.Management.Business.Services.MessageServices;
using Zenit.Management.Contract.Requests.MessageRequests;


namespace Zenit.Management.Host.RequestHandlers
{
    public class GetAllMessagesHandler(MessageService messageService) : IRequestHandler<GetAllMessageRequest, GetAllMessageResponse>
    {
        public async Task<GetAllMessageResponse> Handle(GetAllMessageRequest request, CancellationToken cancellationToken)
        {
            return await messageService.GetAll(request);
        }
    }
}
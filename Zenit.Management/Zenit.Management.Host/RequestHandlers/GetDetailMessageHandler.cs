using MediatR;

using Zenit.Management.Business.Services.MessageServices;
using Zenit.Management.Contract.Requests.MessageRequests;


namespace Zenit.Management.Host.RequestHandlers
{
    public class GetDetailMessageHandler(MessageService messageService) : IRequestHandler<GetDetailMessageRequest, GetDetailMessageResponse>
    {
        public async Task<GetDetailMessageResponse> Handle(GetDetailMessageRequest request, CancellationToken cancellationToken)
        {
            return await messageService.GetDetail(request);
        }
    }
}
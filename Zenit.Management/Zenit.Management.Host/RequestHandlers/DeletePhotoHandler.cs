using MediatR;

using Zenit.Management.Business.Services.PhotoServices;
using Zenit.Management.Contract.Requests.PhotoRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class DeletePhotoHandler(PhotoService photoService) : IRequestHandler<DeletePhotoRequest>
    {
        public async Task Handle(DeletePhotoRequest request, CancellationToken cancellationToken)
        {
            await photoService.Delete(request);
        }
    }
}
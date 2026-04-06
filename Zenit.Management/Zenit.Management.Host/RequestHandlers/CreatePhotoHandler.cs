using MediatR;

using Zenit.Management.Business.Services.PhotoServices;
using Zenit.Management.Contract.Requests.PhotoRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class CreatePhotoHandler(PhotoService photoService) : IRequestHandler<CreatePhotoRequest, CreatePhotoResponse>
    {
        public async Task<CreatePhotoResponse> Handle(CreatePhotoRequest request, CancellationToken cancellationToken)
        {
            return await photoService.Create(request);
        }
    }
}
using MediatR;

using Zenit.Management.Business.Services.PhotoServices;
using Zenit.Management.Contract.Requests.PhotoRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class UpdatePhotoHandler(PhotoService photoService) : IRequestHandler<UpdatePhotoRequest, UpdatePhotoResponse>
    {
        public async Task<UpdatePhotoResponse> Handle(UpdatePhotoRequest request, CancellationToken cancellationToken)
        {
            return await photoService.Update(request);
        }
    }
}
using MediatR;

using Zenit.Management.Business.Services.PhotoServices;
using Zenit.Management.Contract.Requests.PhotoRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class GetAllPhotoHandler(PhotoService photoService) : IRequestHandler<GetAllPhotoRequest, GetAllPhotoResponse>
    {
        public async Task<GetAllPhotoResponse> Handle(GetAllPhotoRequest request, CancellationToken cancellationToken)
        {
            return await photoService.GetAll(request);
        }
    }
}
using MediatR;

using Zenit.Management.Business.Services.PhotoServices;
using Zenit.Management.Contract.Requests.PhotoRequests;

namespace Zenit.Management.Host.RequestHandlers
{
    public class GetDetailPhotoHandler(PhotoService photoService) : IRequestHandler<GetDetailPhotoRequest, GetDetailPhotoResponse>
    {
        public async Task<GetDetailPhotoResponse> Handle(GetDetailPhotoRequest request, CancellationToken cancellationToken)
        {
            return await photoService.GetDetail(request);
        }
    }
}
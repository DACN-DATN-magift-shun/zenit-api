using Zenit.Management.Business.Managers.PhotoManager;
using Zenit.Management.Contract.Requests.PhotoRequests;

namespace Zenit.Management.Business.Services.PhotoServices
{
    public class PhotoService(IServiceProvider serviceProvider) : ManagementApplicationService(serviceProvider)
    {
        private PhotoManager _PhotoManager => GetService<PhotoManager>();

        public async Task<CreatePhotoResponse> Create(CreatePhotoRequest request)
        {
            
        }

        public async Task<UpdatePhotoResponse> Update(UpdatePhotoRequest request)
        {
        }

        public async Task Delete(DeletePhotoRequest request)
        {
        }

        public Task<GetAllPhotoResponse> GetAll(GetAllPhotoRequest request)
        {
        }

        public Task<GetDetailPhotoResponse> GetDetail(GetDetailPhotoRequest request)
        {
        }
    }
}
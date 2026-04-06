using Zenit.Management.Data.Entities;
using Zenit.Share.Data.Interfaces;

namespace Zenit.Management.Business.Managers.PhotoManager
{
    public class PhotoManager(IRepository<Photo> repository) : ManagementDomainService<Photo>(repository)
    {
    }
}
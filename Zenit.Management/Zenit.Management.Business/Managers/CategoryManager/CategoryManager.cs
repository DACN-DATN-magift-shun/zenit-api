using Zenit.Management.Data;
using Zenit.Management.Data.Entities;
using Zenit.Share.Data.Interfaces;

namespace Zenit.Management.Business.Managers.CategoryManager
{
    public class CategoryManager(IRepository<Category> repository) : ManagementDomainService<Category>(repository)
    {
    }
}
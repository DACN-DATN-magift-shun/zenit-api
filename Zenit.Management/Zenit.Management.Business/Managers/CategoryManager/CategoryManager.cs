using Zenit.Management.Data;
using Zenit.Management.Data.Entities;

namespace Zenit.Management.Business.Managers.CategoryManager
{
    public class CategoryManager(ManagementRepository<Category> repository) : ManagementDomainService<Category>(repository)
    {
    }
}
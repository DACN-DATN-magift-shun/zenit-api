using Zenit.Management.Data;
using Zenit.Management.Data.Entities;

namespace Zenit.Management.Business.Managers.CategoryManager
{
    public class CategoryGroupManager(ManagementRepository<CategoryGroup> repository) : ManagementDomainService<CategoryGroup>(repository)
    {
    }
}
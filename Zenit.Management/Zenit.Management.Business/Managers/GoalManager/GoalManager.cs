using Zenit.Management.Data.Entities;
using Zenit.Share.Data.Interfaces;

namespace Zenit.Management.Business.Managers.GoalManager
{
    public class GoalManager(IRepository<Goal> repository) : ManagementDomainService<Goal>(repository)
    {
    }
}
using Zenit.Management.Data.Entities;
using Zenit.Share.Data.Interfaces;

namespace Zenit.Management.Business.Managers.MessageManager
{
    public class MessageManager(IRepository<Message> repository) : ManagementDomainService<Message>(repository)
    {
    }
}
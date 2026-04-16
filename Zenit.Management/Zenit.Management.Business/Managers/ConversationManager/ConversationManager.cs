using Zenit.Management.Data.Entities;
using Zenit.Share.Data.Interfaces;

namespace Zenit.Management.Business.Managers.ConversationManager
{
    public class ConversationManager(IRepository<Conversation> repository) : ManagementDomainService<Conversation>(repository)
    {
    }
}
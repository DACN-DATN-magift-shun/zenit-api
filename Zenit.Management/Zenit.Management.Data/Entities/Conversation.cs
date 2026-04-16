using Zenit.Management.Data.Models;

namespace Zenit.Management.Data.Entities
{
    public class Conversation : ManagementAuditModel
    {
        public required string Title { get; set; }
        public required Guid AccountId { get; set; }
    }
}
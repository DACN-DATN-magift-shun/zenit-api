using Zenit.Management.Data.Models;

namespace Zenit.Management.Data.Entities
{
    public class Message : ManagementAuditModel
    {
        public required Guid ConversationId { get; set; }
        public required Guid AccountId { get; set; }
        public required string Text { get; set; }
    }
}
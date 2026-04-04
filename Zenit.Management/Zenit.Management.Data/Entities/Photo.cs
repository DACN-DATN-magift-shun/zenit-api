using Zenit.Management.Data.Models;

namespace Zenit.Management.Data.Entities
{
    public class Photo : ManagementAuditModel
    {
        public string? FileName { get; set; }
        public string? RelativePath { get; set; }
        public decimal? Size { get; set; }
        public string? ContentType { get; set; }
        public Guid AccountId { get; set; }
        public Guid? TransactionId { get; set; }
    }
}
using Zenit.Share.Data.Interfaces;


namespace Zenit.Share.Data.Models
{
    public abstract class AuditModel : IFullAuditModel
    {
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }
}

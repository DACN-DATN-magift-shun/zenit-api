using Share.Data.Interfaces;


namespace Share.Data.Models
{
    public abstract class AuditModel : IFullAuditModel
    {
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }
}
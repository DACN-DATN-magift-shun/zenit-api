using Share.Data.Interfaces;


namespace Share.Data.Models
{
    public abstract class AuditModel<TID> : IFullAuditModel<TID> where TID : struct
    {
        public TID? CreatedId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public TID? DeletedId { get; set; }
        public DateTime? DeletedAt { get; set; }
        public TID? LastModifiedId { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }
}
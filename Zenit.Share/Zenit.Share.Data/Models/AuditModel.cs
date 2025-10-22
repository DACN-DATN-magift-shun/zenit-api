using Zenit.Share.Data.Interfaces;


namespace Zenit.Share.Data.Models
{
    public abstract class AuditModel<TID> : DataModelBase<TID>, IFullAuditModel<TID>
        where TID : struct
    {
        public TID Id { get; set; }
        public TID? CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public TID? DeletedById { get; set; }
        public DateTime? DeletedAt { get; set; }
        public TID? ModifiedById { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }
}

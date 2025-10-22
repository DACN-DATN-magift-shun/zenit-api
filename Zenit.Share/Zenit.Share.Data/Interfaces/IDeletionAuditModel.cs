namespace Zenit.Share.Data.Interfaces
{
    public interface IDeletionAuditModel
    {
    }

    public interface IDeletionAuditModel<TID> : IDeletionAuditModel
        where TID : struct
    {
        bool IsDeleted { get; set; }
        TID? DeletedById { get; set; }
        DateTime? DeletedAt { get; set; }
    }
}

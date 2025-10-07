namespace Zenit.Share.Data.Interfaces
{
    public interface IDeletionAuditModel
    {
        bool IsDeleted { get; set; }
        DateTime? DeletedAt { get; set; }
    }
}

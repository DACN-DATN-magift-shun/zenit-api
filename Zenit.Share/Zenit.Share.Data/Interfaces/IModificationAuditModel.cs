namespace Zenit.Share.Data.Interfaces
{
    public interface IModificationAuditModel
    {
    }

    public interface IModificationAuditModel<TID> : IModificationAuditModel
        where TID : struct
    {
        TID? ModifiedById { get; set; }
        DateTime? LastModifiedAt { get; set; }
    }
}

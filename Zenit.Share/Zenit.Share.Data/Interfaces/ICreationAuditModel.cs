namespace Zenit.Share.Data.Interfaces
{
    public interface ICreationAuditModel
    {
    }

    public interface ICreationAuditModel<TID> : ICreationAuditModel
        where TID : struct
    {
        TID? CreatedById { get; set; }
        DateTime CreatedAt { get; set; }
    }
    

}

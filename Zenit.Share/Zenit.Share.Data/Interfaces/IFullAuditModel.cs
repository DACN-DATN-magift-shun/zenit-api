namespace Zenit.Share.Data.Interfaces
{
    public interface IFullAuditModel
    {
    }

    public interface IFullAuditModel<TID> : IFullAuditModel, ICreationAuditModel<TID>, IModificationAuditModel<TID>, IDeletionAuditModel<TID>
        where TID : struct
    {
    }
}

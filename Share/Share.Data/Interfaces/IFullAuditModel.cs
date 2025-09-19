using System;


namespace Share.Data.Interfaces
{
    public interface IFullAuditModel<TID> : ICreationAuditModel<TID>, IModificationAuditModel<TID>, IDeletionAuditModel<TID> where TID : struct
    {

    }
}
using System;


namespace Share.Data.Interfaces
{
    public interface IFullAuditModel : ICreationAuditModel, IModificationAuditModel, IDeletionAuditModel
    {

    }
}
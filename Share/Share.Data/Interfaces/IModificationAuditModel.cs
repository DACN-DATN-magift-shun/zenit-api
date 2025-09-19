using System;


namespace Share.Data.Interfaces
{
    public interface IModificationAuditModel<TID> where TID : struct
    {
        TID? LastModifiedId { get; set; }
        DateTime? LastModifiedAt { get; set; }
    }
}
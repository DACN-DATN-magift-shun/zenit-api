using System;


namespace Share.Data.Interfaces
{
    public interface IDeletionAuditModel<TID> where TID : struct
    {
        bool IsDeleted { get; set; }
        TID? DeletedId { get; set; }
        DateTime? DeletedAt { get; set; }
    }
}
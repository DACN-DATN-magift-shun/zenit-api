using System;


namespace Share.Data.Interfaces
{
    public interface ICreationAuditModel<TID> where TID : struct
    {
        TID? CreatedId { get; set; }
        DateTime CreatedAt { get; set; }
    }
}
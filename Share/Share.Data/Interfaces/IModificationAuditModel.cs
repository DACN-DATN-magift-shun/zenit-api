using System;


namespace Share.Data.Interfaces
{
    public interface IModificationAuditModel
    {
        DateTime? LastModifiedAt { get; set; }
    }
}
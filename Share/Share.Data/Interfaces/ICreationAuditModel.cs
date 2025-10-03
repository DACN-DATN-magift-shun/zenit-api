using System;


namespace Share.Data.Interfaces
{
    public interface ICreationAuditModel
    {
        DateTime CreatedAt { get; set; }
    }
}
namespace Zenit.Share.Data.Interfaces;

public interface ISeederHistory<TID> : IDataModel<TID>
    where TID : struct
{
    string SeederName { get; set; }
    DateTime CreatedAt { get; set; }
}
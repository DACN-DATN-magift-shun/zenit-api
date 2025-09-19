namespace Share.Data.Interfaces
{
    public interface IDataModel<TID> where TID : struct
    {
        TID Id { get; set; }
    }
}
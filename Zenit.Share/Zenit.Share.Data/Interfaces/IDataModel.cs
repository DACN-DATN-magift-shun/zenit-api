namespace Zenit.Share.Data.Interfaces
{
    public interface IDataModel
    {
    }

    public interface IDataModel<TID> : IDataModel
        where TID : struct
    {
        TID Id { get; set; }
    }
}

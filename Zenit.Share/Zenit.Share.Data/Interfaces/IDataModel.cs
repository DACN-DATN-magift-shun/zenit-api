namespace Zenit.Share.Data.Interfaces
{
    public interface IDataModel<TID>
    {
        TID Id { get; set; }
    }
}

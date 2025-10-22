using Zenit.Share.Data.Interfaces;


namespace Zenit.Share.Data.Models
{
    public abstract class DataModelBase<TID> : IDataModel<TID>
        where TID : struct
    {
        public TID Id { get; set; }
    }
}

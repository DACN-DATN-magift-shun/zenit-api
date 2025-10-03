using Share.Data.Interfaces;


namespace Share.Data.Models
{
    public abstract class DataModelBase<TID> : AuditModel, IDataModel<TID>
    {
        public TID Id { get; set; }
    }
}
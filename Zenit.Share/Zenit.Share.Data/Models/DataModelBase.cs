using Zenit.Share.Data.Interfaces;


namespace Zenit.Share.Data.Models
{
    public abstract class DataModelBase<TID> : AuditModel, IDataModel<TID>
    {
        public TID Id { get; set; }
    }
}

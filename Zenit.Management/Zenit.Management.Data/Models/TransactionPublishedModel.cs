using Zenit.Share.Common.Enums;

namespace Zenit.Management.Data.Models
{
    public class TransactionPublishedModel
    {
        public required int Amount { get; set; }
        public required DateTime TransactionDate { get; set; }
        public required Guid CategoryId { get; set; }
        public required Guid AccountId { get; set; }
        public required CategoryGroupType GroupType { get; set; }
    }
}
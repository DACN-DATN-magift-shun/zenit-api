using Zenit.Share.Common.Enums;

namespace Zenit.Statistics.Data.Models
{
    public class TransactionModel
    {
        public required int Amount { get; set; }
        public required DateTime TransactionDate { get; set; }
        public required Guid CategoryId { get; set; }
        public required Guid AccountId { get; set; }
        public required CategoryGroupType GroupType { get; set; }
    }
}
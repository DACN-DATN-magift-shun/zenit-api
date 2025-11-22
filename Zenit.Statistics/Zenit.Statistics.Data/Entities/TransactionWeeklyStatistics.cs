using Zenit.Share.Common.Enums;
using Zenit.Statistics.Data.Models;

namespace Zenit.Statistics.Data.Entities;
public class CategoryWeeklyStatistics : StatisticsAuditModel
{
    public required DateTime Date { get; set; }
    public required long TotalAmount { get; set; }
    public float? Percentage { get; set; }
    public float? PercentageChange { get; set; }
    public required Guid CategoryId { get; set; }
    public required Guid GroupId { get; set; }
    public required Guid AccountId { get; set; }
}

public class CategoryGroupWeeklyStatistics : StatisticsAuditModel
{
    public required DateTime Date { get; set; }
    public required long TotalAmount { get; set; }
    public float? Percentage { get; set; }
    public float? PercentageChange { get; set; }
    public required Guid AccountId { get; set; }
    public required CategoryGroupType GroupType { get; set; }
}

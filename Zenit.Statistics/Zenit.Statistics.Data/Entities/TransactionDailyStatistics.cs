using Microsoft.EntityFrameworkCore;

using Zenit.Share.Common.Enums;
using Zenit.Statistics.Data.Models;

namespace Zenit.Statistics.Data.Entities;

[Index(nameof(Date), nameof(CategoryId), IsUnique = true)]
public class CategoryDailyStatistics : StatisticsAuditModel
{
    public required DateTime Date { get; set; }
    public required long TotalAmount { get; set; }
    public float? Percentage { get; set; }
    public float? PercentageChange { get; set; }
    public required Guid CategoryId { get; set; }
    public required Guid GroupId { get; set; }
    public required Guid AccountId { get; set; }
}

[Index(nameof(Date), nameof(GroupType), nameof(AccountId), IsUnique = true)]
public class CategoryGroupDailyStatistics : StatisticsAuditModel
{
    public required DateTime Date { get; set; }
    public required long TotalAmount { get; set; }
    public float? Percentage { get; set; }
    public float? PercentageChange { get; set; }
    public required Guid AccountId { get; set; }
    public required CategoryGroupType GroupType { get; set; }
}

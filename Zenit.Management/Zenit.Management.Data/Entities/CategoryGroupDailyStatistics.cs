using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Zenit.Management.Data.Models;
using Zenit.Share.Common.Enums;

[Index(nameof(Date), nameof(GroupType), nameof(AccountId), IsUnique = true)]
public class CategoryGroupDailyStatistics : ManagementAuditModel
{
    [Column(TypeName = "date")]
    public required DateTime Date { get; set; }
    public required long TotalAmount { get; set; }
    public float? Percentage { get; set; }
    public float? PercentageChange { get; set; }
    public required Guid AccountId { get; set; }
    public required CategoryGroupType GroupType { get; set; }
}
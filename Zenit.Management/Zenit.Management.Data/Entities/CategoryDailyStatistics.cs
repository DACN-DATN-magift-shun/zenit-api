using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Zenit.Management.Data.Models;

namespace Zenit.Management.Data.Entities;

[Index(nameof(Date), nameof(CategoryId), IsUnique = true)]
public class CategoryDailyStatistics : ManagementAuditModel
{
    [Column(TypeName = "date")]
    public required DateTime Date { get; set; }
    public required long TotalAmount { get; set; }
    public float? Percentage { get; set; }
    public float? PercentageChange { get; set; }
    public required Guid CategoryId { get; set; }
    public required Guid GroupId { get; set; }
    // public required Guid AccountId { get; set; }
}

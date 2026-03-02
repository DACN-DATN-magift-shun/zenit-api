using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

using Zenit.Management.Contract.Requests.StatisticsRequests;
using Zenit.Share.Common.Enums;

namespace Zenit.Management.Business.Helpers
{
    public class ReportHelper
    {
        public static string GeneratePdfReport(
            IEnumerable<StatisticsResponseItem> groupStatistics,
            IncomeExpenseStatistics incomeExpenseStatistics,
            Guid accountId)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var filePath = Path.Combine(Path.GetTempPath(), $"{accountId}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");

            var stats = groupStatistics.ToList();
            var generatedAt = DateTime.Now;

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                    // ── HEADER ──
                    page.Header().Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(inner =>
                            {
                                inner.Item()
                                    .Text("💰 Báo Cáo Tài Chính")
                                    .FontSize(24).Bold().FontColor(Colors.Indigo.Darken2);

                                inner.Item()
                                    .Text("(aka: Tiền đâu hết vậy trời?!)")
                                    .FontSize(12).Italic().FontColor(Colors.Grey.Darken1);
                            });

                            row.ConstantItem(140).AlignRight().Column(inner =>
                            {
                                inner.Item()
                                    .Text($"📅 {generatedAt:dd/MM/yyyy}")
                                    .FontSize(10).FontColor(Colors.Grey.Medium);
                                inner.Item()
                                    .Text($"🕐 {generatedAt:HH:mm:ss}")
                                    .FontSize(10).FontColor(Colors.Grey.Medium);
                            });
                        });

                        col.Item().PaddingTop(4).LineHorizontal(2).LineColor(Colors.Indigo.Lighten2);
                        col.Item().PaddingTop(2).PaddingBottom(6)
                            .Text("Được tạo với tất cả tình yêu thương 💖 (và một chút lo lắng về ví tiền)")
                            .FontSize(9).Italic().FontColor(Colors.Grey.Medium);
                    });

                    // ── CONTENT ──
                    page.Content().Column(col =>
                    {
                        // === TỔNG QUAN THU CHI ===
                        col.Item().PaddingBottom(8)
                            .Text("📊 Tổng Quan Thu Chi — Bức Tranh Toàn Cảnh")
                            .FontSize(15).Bold().FontColor(Colors.Indigo.Darken1);

                        col.Item().PaddingBottom(12).Row(row =>
                        {
                            // Thu nhập
                            row.RelativeItem().Border(1).BorderColor(Colors.Green.Lighten2)
                                .Background(Colors.Green.Lighten5).Padding(12).Column(inner =>
                                {
                                    inner.Item().Text("🤑 Tổng Thu Nhập").Bold().FontColor(Colors.Green.Darken2);
                                    inner.Item().PaddingTop(4)
                                        .Text(FormatMoney(incomeExpenseStatistics.TotalIncome))
                                        .FontSize(18).Bold().FontColor(Colors.Green.Darken3);

                                    if (incomeExpenseStatistics.IncomePercentageChange.HasValue)
                                    {
                                        var change = incomeExpenseStatistics.IncomePercentageChange.Value;
                                        var emoji = change >= 0 ? "📈" : "📉";
                                        var color = change >= 0 ? Colors.Green.Darken1 : Colors.Red.Darken1;
                                        inner.Item().PaddingTop(2)
                                            .Text($"{emoji} {change:+0.##;-0.##}% so với kỳ trước")
                                            .FontSize(9).FontColor(color);
                                    }

                                    inner.Item().PaddingTop(4)
                                        .Text(incomeExpenseStatistics.TotalIncome > 5_000_000
                                            ? "Không tệ đấy bạn ơi! 🎉"
                                            : "Cố lên nào! 💪")
                                        .FontSize(9).Italic().FontColor(Colors.Grey.Darken1);
                                });

                            row.ConstantItem(12);

                            // Chi tiêu
                            row.RelativeItem().Border(1).BorderColor(Colors.Red.Lighten2)
                                .Background(Colors.Red.Lighten5).Padding(12).Column(inner =>
                                {
                                    inner.Item().Text("😅 Tổng Chi Tiêu").Bold().FontColor(Colors.Red.Darken2);
                                    inner.Item().PaddingTop(4)
                                        .Text(FormatMoney(incomeExpenseStatistics.TotalExpense))
                                        .FontSize(18).Bold().FontColor(Colors.Red.Darken3);

                                    if (incomeExpenseStatistics.ExpensePercentageChange.HasValue)
                                    {
                                        var change = incomeExpenseStatistics.ExpensePercentageChange.Value;
                                        var emoji = change >= 0 ? "📈" : "📉";
                                        var color = change >= 0 ? Colors.Red.Darken1 : Colors.Green.Darken1;
                                        inner.Item().PaddingTop(2)
                                            .Text($"{emoji} {change:+0.##;-0.##}% so với kỳ trước")
                                            .FontSize(9).FontColor(color);
                                    }

                                    var expenseComment = GetExpenseComment(
                                        incomeExpenseStatistics.TotalIncome,
                                        incomeExpenseStatistics.TotalExpense);
                                    inner.Item().PaddingTop(4)
                                        .Text(expenseComment)
                                        .FontSize(9).Italic().FontColor(Colors.Grey.Darken1);
                                });
                        });

                        // Số dư
                        var balance = incomeExpenseStatistics.TotalIncome - incomeExpenseStatistics.TotalExpense;
                        var balanceColor = balance >= 0 ? Colors.Green.Darken2 : Colors.Red.Darken2;
                        var balanceBg = balance >= 0 ? Colors.Green.Lighten4 : Colors.Red.Lighten4;
                        var balanceBorder = balance >= 0 ? Colors.Green.Lighten1 : Colors.Red.Lighten1;

                        col.Item().PaddingBottom(16)
                            .Border(1).BorderColor(balanceBorder)
                            .Background(balanceBg).Padding(10).Row(row =>
                            {
                                row.RelativeItem()
                                    .Text(balance >= 0
                                        ? $"🏦 Số Dư Còn Lại: {FormatMoney(balance)}  — Bạn đang làm tốt lắm! Giữ vững nhé!"
                                        : $"🚨 Bội Chi: {FormatMoney(Math.Abs(balance))}  — Ơ kìa... ví đang khóc đó bạn ơi 😭")
                                    .FontSize(12).Bold().FontColor(balanceColor);
                            });

                        // === CHI TIẾT THEO NHÓM ===
                        if (stats.Any())
                        {
                            col.Item().PaddingBottom(8)
                                .Text("🗂️ Chi Tiết Theo Nhóm — Ai Ngốn Nhiều Nhất Nào?")
                                .FontSize(15).Bold().FontColor(Colors.Indigo.Darken1);

                            foreach (var group in stats)
                            {
                                col.Item().PaddingBottom(10).Border(1).BorderColor(Colors.Grey.Lighten2)
                                    .Column(groupCol =>
                                    {
                                        // Group header
                                        groupCol.Item()
                                            .Background(Colors.Indigo.Lighten4).Padding(8)
                                            .Row(headerRow =>
                                            {
                                                headerRow.RelativeItem()
                                                    .Text(GetGroupTitle(group.GroupType))
                                                    .FontSize(12).Bold().FontColor(Colors.Indigo.Darken2);

                                                headerRow.ConstantItem(180).AlignRight().Column(amtCol =>
                                                {
                                                    amtCol.Item()
                                                        .Text(FormatMoney(group.TotalAmount))
                                                        .FontSize(12).Bold().FontColor(Colors.Indigo.Darken3);

                                                    if (group.Percentage.HasValue)
                                                        amtCol.Item()
                                                            .Text($"Chiếm {group.Percentage.Value:0.##}% tổng chi")
                                                            .FontSize(9).FontColor(Colors.Grey.Darken1);

                                                    if (group.PercentageChange.HasValue)
                                                    {
                                                        var ch = group.PercentageChange.Value;
                                                        amtCol.Item()
                                                            .Text($"{(ch >= 0 ? "▲" : "▼")} {ch:+0.##;-0.##}% kỳ trước")
                                                            .FontSize(9)
                                                            .FontColor(ch >= 0 ? Colors.Red.Darken1 : Colors.Green.Darken1);
                                                    }
                                                });
                                            });

                                        // Category rows
                                        var categories = group.Categories.ToList();
                                        if (categories.Any())
                                        {
                                            groupCol.Item().Table(table =>
                                            {
                                                table.ColumnsDefinition(cols =>
                                                {
                                                    cols.RelativeColumn(3);
                                                    cols.RelativeColumn(2);
                                                    cols.RelativeColumn(1.5f);
                                                    cols.RelativeColumn(1.5f);
                                                });

                                                // Table header
                                                table.Header(h =>
                                                {
                                                    h.Cell().Background(Colors.Grey.Lighten3).Padding(5)
                                                        .Text("Danh Mục").FontSize(10).Bold();
                                                    h.Cell().Background(Colors.Grey.Lighten3).Padding(5).AlignRight()
                                                        .Text("Số Tiền").FontSize(10).Bold();
                                                    h.Cell().Background(Colors.Grey.Lighten3).Padding(5).AlignRight()
                                                        .Text("Tỷ Lệ").FontSize(10).Bold();
                                                    h.Cell().Background(Colors.Grey.Lighten3).Padding(5).AlignRight()
                                                        .Text("Thay Đổi").FontSize(10).Bold();
                                                });

                                                bool isAlt = false;
                                                foreach (var cat in categories)
                                                {
                                                    var bg = isAlt ? Colors.Grey.Lighten5 : Colors.White;
                                                    isAlt = !isAlt;

                                                    table.Cell().Background(bg).Padding(5)
                                                        .Text(cat.CategoryName ?? "🤷 Không rõ").FontSize(10);

                                                    table.Cell().Background(bg).Padding(5).AlignRight()
                                                        .Text(FormatMoney(cat.TotalAmount)).FontSize(10);

                                                    table.Cell().Background(bg).Padding(5).AlignRight()
                                                        .Text(cat.Percentage.HasValue
                                                            ? $"{cat.Percentage.Value:0.##}%"
                                                            : "—")
                                                        .FontSize(10).FontColor(Colors.Grey.Darken1);

                                                    if (cat.PercentageChange.HasValue)
                                                    {
                                                        var ch = cat.PercentageChange.Value;
                                                        table.Cell().Background(bg).Padding(5).AlignRight()
                                                            .Text($"{(ch >= 0 ? "▲" : "▼")} {ch:+0.##;-0.##}%")
                                                            .FontSize(10)
                                                            .FontColor(ch >= 0 ? Colors.Red.Darken1 : Colors.Green.Darken1);
                                                    }
                                                    else
                                                    {
                                                        table.Cell().Background(bg).Padding(5).AlignRight()
                                                            .Text("—").FontSize(10).FontColor(Colors.Grey.Lighten1);
                                                    }
                                                }
                                            });
                                        }
                                        else
                                        {
                                            groupCol.Item().Padding(8)
                                                .Text("😶 Nhóm này không có danh mục con nào. Bí ẩn thật!")
                                                .FontSize(10).Italic().FontColor(Colors.Grey.Medium);
                                        }
                                    });
                            }
                        }
                        else
                        {
                            col.Item().Padding(20).AlignCenter()
                                .Text("🦗 Không có dữ liệu nào cả... Nghe có vẻ yên bình nhưng cũng hơi đáng ngờ 🧐")
                                .FontSize(12).Italic().FontColor(Colors.Grey.Medium);
                        }
                    });

                    // ── FOOTER ──
                    page.Footer().BorderTop(1).BorderColor(Colors.Grey.Lighten2).PaddingTop(6).Row(row =>
                    {
                        row.RelativeItem()
                            .Text("🤖 Báo cáo này được tạo tự động — đừng trách máy nếu số liệu xấu nhé!")
                            .FontSize(8).Italic().FontColor(Colors.Grey.Medium);

                        row.ConstantItem(60).AlignRight()
                            .Text(x =>
                            {
                                x.Span("Trang ").FontSize(8).FontColor(Colors.Grey.Medium);
                                x.CurrentPageNumber().FontSize(8).FontColor(Colors.Grey.Medium);
                                x.Span(" / ").FontSize(8).FontColor(Colors.Grey.Medium);
                                x.TotalPages().FontSize(8).FontColor(Colors.Grey.Medium);
                            });
                    });
                });
            })
            .GeneratePdf(filePath);

            return filePath;
        }

        // ── Helpers ──

        private static string FormatMoney(long amount) =>
            $"{amount:N0} đ";

        private static string GetGroupTitle(CategoryGroupType? groupType) =>
            groupType switch
            {
                CategoryGroupType.Neccessary        => "🏠 Chi Phí Thiết Yếu",
                CategoryGroupType.Income           => "💵 Thu Nhập",
                CategoryGroupType.SelfDevelopment => "📚 Phát Triển Bản Thân",
                CategoryGroupType.Assets   => "💎 Tài Sản",
                CategoryGroupType.Entertainment => "🎮 Giải Trí",
                _                                 => "📦 Nhóm Khác"  // wildcard tránh crash
            };

        private static string GetExpenseComment(long income, long expense)
        {
            if (income == 0) return "Chưa có thu nhập, còn gì để chi nữa?! 😶";
            var ratio = (double)expense / income;
            return ratio switch
            {
                <= 0.5  => "Tiết kiệm như thần! Bạn có bí quyết gì vậy? 🧙",
                <= 0.75 => "Khá ổn đó, tiếp tục duy trì nhé! 👍",
                <= 0.9  => "Hơi căng rồi đó bạn ơi... 😬",
                <= 1.0  => "Xài gần hết rồi! Cẩn thận kẻo vỡ ví 😰",
                _       => "Ơ kìa... chi nhiều hơn kiếm à?! 😱"
            };
        }
    }
}
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenit.Statistics.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class DeleteIndexOfCategoryDailyStatistics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CategoryDailyStatistics_Date_GroupId",
                table: "CategoryDailyStatistics");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CategoryDailyStatistics_Date_GroupId",
                table: "CategoryDailyStatistics",
                columns: new[] { "Date", "GroupId" },
                unique: true);
        }
    }
}

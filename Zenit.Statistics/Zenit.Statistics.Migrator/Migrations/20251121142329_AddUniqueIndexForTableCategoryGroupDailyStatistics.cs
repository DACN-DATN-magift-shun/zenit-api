using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenit.Statistics.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexForTableCategoryGroupDailyStatistics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.CreateIndex(
            //     name: "IX_CategoryGroupDailyStatistics_Date_GroupType_AccountId",
            //     table: "CategoryGroupDailyStatistics",
            //     columns: new[] { "Date", "GroupType", "AccountId" },
            //     unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CategoryGroupDailyStatistics_Date_GroupType_AccountId",
                table: "CategoryGroupDailyStatistics");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenit.Statistics.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransactionStatisticsIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TransactionStatistics_Date_CategoryId",
                table: "TransactionStatistics");

            migrationBuilder.DropIndex(
                name: "IX_TransactionStatistics_Date_GroupType",
                table: "TransactionStatistics");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStatistics_Date_CategoryId",
                table: "TransactionStatistics",
                columns: new[] { "Date", "CategoryId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStatistics_Date_GroupType",
                table: "TransactionStatistics",
                columns: new[] { "Date", "GroupType" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TransactionStatistics_Date_CategoryId",
                table: "TransactionStatistics");

            migrationBuilder.DropIndex(
                name: "IX_TransactionStatistics_Date_GroupType",
                table: "TransactionStatistics");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStatistics_Date_CategoryId",
                table: "TransactionStatistics",
                columns: new[] { "Date", "CategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStatistics_Date_GroupType",
                table: "TransactionStatistics",
                columns: new[] { "Date", "GroupType" });
        }
    }
}

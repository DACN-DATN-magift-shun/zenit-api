using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenit.Management.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultSchemaAsManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "management");

            migrationBuilder.RenameTable(
                name: "Transaction",
                newName: "Transaction",
                newSchema: "management");

            migrationBuilder.RenameTable(
                name: "ManagementSeederHistory",
                newName: "ManagementSeederHistory",
                newSchema: "management");

            migrationBuilder.RenameTable(
                name: "CategorySettings",
                newName: "CategorySettings",
                newSchema: "management");

            migrationBuilder.RenameTable(
                name: "CategoryGroupSettings",
                newName: "CategoryGroupSettings",
                newSchema: "management");

            migrationBuilder.RenameTable(
                name: "CategoryGroupDailyStatistics",
                newName: "CategoryGroupDailyStatistics",
                newSchema: "management");

            migrationBuilder.RenameTable(
                name: "CategoryGroup",
                newName: "CategoryGroup",
                newSchema: "management");

            migrationBuilder.RenameTable(
                name: "CategoryDailyStatistics",
                newName: "CategoryDailyStatistics",
                newSchema: "management");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Category",
                newSchema: "management");

            migrationBuilder.RenameTable(
                name: "Account",
                newName: "Account",
                newSchema: "management");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Transaction",
                schema: "management",
                newName: "Transaction");

            migrationBuilder.RenameTable(
                name: "ManagementSeederHistory",
                schema: "management",
                newName: "ManagementSeederHistory");

            migrationBuilder.RenameTable(
                name: "CategorySettings",
                schema: "management",
                newName: "CategorySettings");

            migrationBuilder.RenameTable(
                name: "CategoryGroupSettings",
                schema: "management",
                newName: "CategoryGroupSettings");

            migrationBuilder.RenameTable(
                name: "CategoryGroupDailyStatistics",
                schema: "management",
                newName: "CategoryGroupDailyStatistics");

            migrationBuilder.RenameTable(
                name: "CategoryGroup",
                schema: "management",
                newName: "CategoryGroup");

            migrationBuilder.RenameTable(
                name: "CategoryDailyStatistics",
                schema: "management",
                newName: "CategoryDailyStatistics");

            migrationBuilder.RenameTable(
                name: "Category",
                schema: "management",
                newName: "Category");

            migrationBuilder.RenameTable(
                name: "Account",
                schema: "management",
                newName: "Account");
        }
    }
}

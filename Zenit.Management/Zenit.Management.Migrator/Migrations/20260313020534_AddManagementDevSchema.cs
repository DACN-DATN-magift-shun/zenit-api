using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenit.Management.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class AddManagementDevSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "zenit_management_dev");

            migrationBuilder.RenameTable(
                name: "Transaction",
                schema: "management",
                newName: "Transaction",
                newSchema: "zenit_management_dev");

            migrationBuilder.RenameTable(
                name: "ManagementSeederHistory",
                schema: "management",
                newName: "ManagementSeederHistory",
                newSchema: "zenit_management_dev");

            migrationBuilder.RenameTable(
                name: "CategorySettings",
                schema: "management",
                newName: "CategorySettings",
                newSchema: "zenit_management_dev");

            migrationBuilder.RenameTable(
                name: "CategoryGroupSettings",
                schema: "management",
                newName: "CategoryGroupSettings",
                newSchema: "zenit_management_dev");

            migrationBuilder.RenameTable(
                name: "CategoryGroupDailyStatistics",
                schema: "management",
                newName: "CategoryGroupDailyStatistics",
                newSchema: "zenit_management_dev");

            migrationBuilder.RenameTable(
                name: "CategoryGroup",
                schema: "management",
                newName: "CategoryGroup",
                newSchema: "zenit_management_dev");

            migrationBuilder.RenameTable(
                name: "CategoryDailyStatistics",
                schema: "management",
                newName: "CategoryDailyStatistics",
                newSchema: "zenit_management_dev");

            migrationBuilder.RenameTable(
                name: "Category",
                schema: "management",
                newName: "Category",
                newSchema: "zenit_management_dev");

            migrationBuilder.RenameTable(
                name: "Account",
                schema: "management",
                newName: "Account",
                newSchema: "zenit_management_dev");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "management");

            migrationBuilder.RenameTable(
                name: "Transaction",
                schema: "zenit_management_dev",
                newName: "Transaction",
                newSchema: "management");

            migrationBuilder.RenameTable(
                name: "ManagementSeederHistory",
                schema: "zenit_management_dev",
                newName: "ManagementSeederHistory",
                newSchema: "management");

            migrationBuilder.RenameTable(
                name: "CategorySettings",
                schema: "zenit_management_dev",
                newName: "CategorySettings",
                newSchema: "management");

            migrationBuilder.RenameTable(
                name: "CategoryGroupSettings",
                schema: "zenit_management_dev",
                newName: "CategoryGroupSettings",
                newSchema: "management");

            migrationBuilder.RenameTable(
                name: "CategoryGroupDailyStatistics",
                schema: "zenit_management_dev",
                newName: "CategoryGroupDailyStatistics",
                newSchema: "management");

            migrationBuilder.RenameTable(
                name: "CategoryGroup",
                schema: "zenit_management_dev",
                newName: "CategoryGroup",
                newSchema: "management");

            migrationBuilder.RenameTable(
                name: "CategoryDailyStatistics",
                schema: "zenit_management_dev",
                newName: "CategoryDailyStatistics",
                newSchema: "management");

            migrationBuilder.RenameTable(
                name: "Category",
                schema: "zenit_management_dev",
                newName: "Category",
                newSchema: "management");

            migrationBuilder.RenameTable(
                name: "Account",
                schema: "zenit_management_dev",
                newName: "Account",
                newSchema: "management");
        }
    }
}

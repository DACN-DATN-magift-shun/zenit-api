using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenit.Management.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class UpdateManagementTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.RenameColumn(
            //     name: "UserId",
            //     table: "Transaction",
            //     newName: "AccountId");

            // migrationBuilder.RenameColumn(
            //     name: "UserId",
            //     table: "CategoryGroupExpenseAlertThreshold",
            //     newName: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Transaction",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "CategoryGroupExpenseAlertThreshold",
                newName: "UserId");
        }
    }
}

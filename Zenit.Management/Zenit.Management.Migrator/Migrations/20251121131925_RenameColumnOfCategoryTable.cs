using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenit.Management.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumnOfCategoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.RenameColumn(
            //     name: "UserId",
            //     table: "Category",
            //     newName: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Category",
                newName: "UserId");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenit.Management.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class AddColorColumnsToCategoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.AddColumn<string>(
            //     name: "BackgroundColor",
            //     table: "Category",
            //     type: "text",
            //     nullable: false,
            //     defaultValue: "");

            // migrationBuilder.AddColumn<string>(
            //     name: "Color",
            //     table: "Category",
            //     type: "text",
            //     nullable: false,
            //     defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackgroundColor",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Category");
        }
    }
}

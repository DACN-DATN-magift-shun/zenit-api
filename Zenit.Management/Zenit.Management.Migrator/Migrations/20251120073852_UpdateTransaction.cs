using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenit.Management.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.CreateIndex(
            //     name: "IX_Transaction_CategoryId",
            //     table: "Transaction",
            //     column: "CategoryId");

            // migrationBuilder.AddForeignKey(
            //     name: "FK_Transaction_Category_CategoryId",
            //     table: "Transaction",
            //     column: "CategoryId",
            //     principalTable: "Category",
            //     principalColumn: "Id",
            //     onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Category_CategoryId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_CategoryId",
                table: "Transaction");
        }
    }
}

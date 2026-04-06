using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenit.Management.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePhotoTableSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RelativePath",
                schema: "zenit_management_dev",
                table: "Photo");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                schema: "zenit_management_dev",
                table: "Photo",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Photo_TransactionId",
                schema: "zenit_management_dev",
                table: "Photo",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photo_Transaction_TransactionId",
                schema: "zenit_management_dev",
                table: "Photo",
                column: "TransactionId",
                principalSchema: "zenit_management_dev",
                principalTable: "Transaction",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photo_Transaction_TransactionId",
                schema: "zenit_management_dev",
                table: "Photo");

            migrationBuilder.DropIndex(
                name: "IX_Photo_TransactionId",
                schema: "zenit_management_dev",
                table: "Photo");

            migrationBuilder.DropColumn(
                name: "FilePath",
                schema: "zenit_management_dev",
                table: "Photo");

            migrationBuilder.AddColumn<string>(
                name: "RelativePath",
                schema: "zenit_management_dev",
                table: "Photo",
                type: "text",
                nullable: true);
        }
    }
}

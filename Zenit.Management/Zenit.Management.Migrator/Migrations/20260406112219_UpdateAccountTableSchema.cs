using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenit.Management.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAccountTableSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountId",
                schema: "zenit_management_dev",
                table: "Photo");

            migrationBuilder.AddColumn<Guid>(
                name: "PhotoId",
                schema: "zenit_management_dev",
                table: "Account",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_PhotoId",
                schema: "zenit_management_dev",
                table: "Account",
                column: "PhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Photo_PhotoId",
                schema: "zenit_management_dev",
                table: "Account",
                column: "PhotoId",
                principalSchema: "zenit_management_dev",
                principalTable: "Photo",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Photo_PhotoId",
                schema: "zenit_management_dev",
                table: "Account");

            migrationBuilder.DropIndex(
                name: "IX_Account_PhotoId",
                schema: "zenit_management_dev",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                schema: "zenit_management_dev",
                table: "Account");

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                schema: "zenit_management_dev",
                table: "Photo",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}

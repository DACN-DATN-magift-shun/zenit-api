using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenit.Management.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class UpdateColumnFromWalletIdOfTransferHistoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MoneyTransferHistory_Wallet_FromWalletId",
                schema: "zenit_management_dev",
                table: "MoneyTransferHistory");

            migrationBuilder.AlterColumn<Guid>(
                name: "FromWalletId",
                schema: "zenit_management_dev",
                table: "MoneyTransferHistory",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_MoneyTransferHistory_Wallet_FromWalletId",
                schema: "zenit_management_dev",
                table: "MoneyTransferHistory",
                column: "FromWalletId",
                principalSchema: "zenit_management_dev",
                principalTable: "Wallet",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MoneyTransferHistory_Wallet_FromWalletId",
                schema: "zenit_management_dev",
                table: "MoneyTransferHistory");

            migrationBuilder.AlterColumn<Guid>(
                name: "FromWalletId",
                schema: "zenit_management_dev",
                table: "MoneyTransferHistory",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MoneyTransferHistory_Wallet_FromWalletId",
                schema: "zenit_management_dev",
                table: "MoneyTransferHistory",
                column: "FromWalletId",
                principalSchema: "zenit_management_dev",
                principalTable: "Wallet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

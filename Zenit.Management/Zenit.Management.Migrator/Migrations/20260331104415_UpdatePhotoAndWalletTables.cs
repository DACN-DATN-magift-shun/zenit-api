using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenit.Management.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePhotoAndWalletTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransferHistory",
                schema: "zenit_management_dev");

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                schema: "zenit_management_dev",
                table: "Wallet",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                schema: "zenit_management_dev",
                table: "Photo",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TransactionId",
                schema: "zenit_management_dev",
                table: "Photo",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MoneyTransferHistory",
                schema: "zenit_management_dev",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FromWalletId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToWalletId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    TransferDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedById = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoneyTransferHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoneyTransferHistory_Wallet_FromWalletId",
                        column: x => x.FromWalletId,
                        principalSchema: "zenit_management_dev",
                        principalTable: "Wallet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MoneyTransferHistory_Wallet_ToWalletId",
                        column: x => x.ToWalletId,
                        principalSchema: "zenit_management_dev",
                        principalTable: "Wallet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MoneyTransferHistory_FromWalletId",
                schema: "zenit_management_dev",
                table: "MoneyTransferHistory",
                column: "FromWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_MoneyTransferHistory_ToWalletId",
                schema: "zenit_management_dev",
                table: "MoneyTransferHistory",
                column: "ToWalletId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoneyTransferHistory",
                schema: "zenit_management_dev");

            migrationBuilder.DropColumn(
                name: "AccountId",
                schema: "zenit_management_dev",
                table: "Wallet");

            migrationBuilder.DropColumn(
                name: "AccountId",
                schema: "zenit_management_dev",
                table: "Photo");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                schema: "zenit_management_dev",
                table: "Photo");

            migrationBuilder.CreateTable(
                name: "TransferHistory",
                schema: "zenit_management_dev",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FromWalletId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToWalletId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedById = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "uuid", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    TransferDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferHistory_Wallet_FromWalletId",
                        column: x => x.FromWalletId,
                        principalSchema: "zenit_management_dev",
                        principalTable: "Wallet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransferHistory_Wallet_ToWalletId",
                        column: x => x.ToWalletId,
                        principalSchema: "zenit_management_dev",
                        principalTable: "Wallet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransferHistory_FromWalletId",
                schema: "zenit_management_dev",
                table: "TransferHistory",
                column: "FromWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferHistory_ToWalletId",
                schema: "zenit_management_dev",
                table: "TransferHistory",
                column: "ToWalletId");
        }
    }
}

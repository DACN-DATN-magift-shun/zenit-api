using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenit.Statistics.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.CreateTable(
            //     name: "TransactionStatistics",
            //     columns: table => new
            //     {
            //         Id = table.Column<Guid>(type: "uuid", nullable: false),
            //         Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         TotalAmount = table.Column<long>(type: "bigint", nullable: false),
            //         Percentage = table.Column<float>(type: "real", nullable: true),
            //         PercentageChange = table.Column<float>(type: "real", nullable: true),
            //         CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
            //         GroupType = table.Column<int>(type: "integer", nullable: false),
            //         AccountId = table.Column<Guid>(type: "uuid", nullable: false),
            //         CreatedById = table.Column<Guid>(type: "uuid", nullable: true),
            //         CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //         IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
            //         DeletedById = table.Column<Guid>(type: "uuid", nullable: true),
            //         DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            //         ModifiedById = table.Column<Guid>(type: "uuid", nullable: true),
            //         LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_TransactionStatistics", x => x.Id);
            //     });

            // migrationBuilder.CreateIndex(
            //     name: "IX_TransactionStatistics_Date_CategoryId",
            //     table: "TransactionStatistics",
            //     columns: new[] { "Date", "CategoryId" });

            // migrationBuilder.CreateIndex(
            //     name: "IX_TransactionStatistics_Date_GroupType",
            //     table: "TransactionStatistics",
            //     columns: new[] { "Date", "GroupType" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionStatistics");
        }
    }
}

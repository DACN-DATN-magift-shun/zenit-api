using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenit.Management.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class CreateCategoryGroupExpenseAlertThresholdTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.CreateTable(
            //     name: "CategoryGroupExpenseAlertThreshold",
            //     columns: table => new
            //     {
            //         Id = table.Column<Guid>(type: "uuid", nullable: false),
            //         GroupType = table.Column<int>(type: "integer", nullable: false),
            //         Threshold = table.Column<float>(type: "real", nullable: true),
            //         UserId = table.Column<Guid>(type: "uuid", nullable: false),
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
            //         table.PrimaryKey("PK_CategoryGroupExpenseAlertThreshold", x => x.Id);
            //     });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryGroupExpenseAlertThreshold");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenit.Management.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.CreateTable(
            //     name: "Account",
            //     columns: table => new
            //     {
            //         Id = table.Column<Guid>(type: "uuid", nullable: false),
            //         Username = table.Column<string>(type: "text", nullable: false),
            //         Email = table.Column<string>(type: "text", nullable: false),
            //         Phone = table.Column<string>(type: "text", nullable: true),
            //         Address = table.Column<string>(type: "text", nullable: true),
            //         Password = table.Column<string>(type: "text", nullable: false),
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
            //         table.PrimaryKey("PK_Account", x => x.Id);
            //     });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}

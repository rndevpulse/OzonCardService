using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OzonCard.Common.Infrastructure.Database.Migrations.Operational
{
    /// <inheritdoc />
    public partial class AddUserIdFieldMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "organizations_members",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "organizations_members");
        }
    }
}

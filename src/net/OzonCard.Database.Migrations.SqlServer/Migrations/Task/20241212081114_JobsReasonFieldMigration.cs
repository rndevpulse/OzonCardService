#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace OzonCard.Database.Migrations.SqlServer.Migrations.Task
{
    /// <inheritdoc />
    public partial class JobsReasonFieldMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reason",
                schema: "task",
                table: "jobs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reason",
                schema: "task",
                table: "jobs");
        }
    }
}

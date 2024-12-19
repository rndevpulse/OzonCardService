#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace OzonCard.Database.Migrations.SqlServer.Migrations.Task
{
    /// <inheritdoc />
    public partial class JobsTitleFieldMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "task",
                table: "jobs");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Closed",
                schema: "task",
                table: "jobs",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ProcessedAt",
                schema: "task",
                table: "jobs",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "task",
                table: "jobs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessedAt",
                schema: "task",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "task",
                table: "jobs");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Closed",
                schema: "task",
                table: "jobs",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                schema: "task",
                table: "jobs",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}

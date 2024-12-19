#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace OzonCard.Database.Migrations.SqlServer.Migrations.Task
{
    /// <inheritdoc />
    public partial class JobsTableMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "task");

            migrationBuilder.CreateTable(
                name: "jobs",
                schema: "task",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    User = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Arguments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Closed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Progress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jobs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "jobs",
                schema: "task");
        }
    }
}

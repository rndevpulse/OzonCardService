using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OzonCard.Common.Infrastructure.Database.Migrations.Operational
{
    /// <inheritdoc />
    public partial class CategoriesFKMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_organizations_categories",
                table: "organizations_categories");

            migrationBuilder.DropIndex(
                name: "IX_organizations_categories_OrganizationId",
                table: "organizations_categories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_organizations_categories",
                table: "organizations_categories",
                columns: new[] { "OrganizationId", "Id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_organizations_categories",
                table: "organizations_categories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_organizations_categories",
                table: "organizations_categories",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_organizations_categories_OrganizationId",
                table: "organizations_categories",
                column: "OrganizationId");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OzonCard.Database.Migrations.SqlServer.Migrations.Operational
{
    /// <inheritdoc />
    public partial class restructuringMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customers_wallets");

            migrationBuilder.DropTable(
                name: "organizations_programs_wallets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_organizations_members",
                table: "organizations_members");

            migrationBuilder.DropPrimaryKey(
                name: "PK_organizations_categories",
                table: "organizations_categories");
            
            migrationBuilder.DropPrimaryKey(
                name: "PK_organizations_programs",
                table: "organizations_programs");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "organizations_programs");
            
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "organizations_programs",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<Guid>(
                name: "ProgramId",
                table: "organizations_programs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WalletId",
                table: "organizations_programs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WalletType",
                table: "organizations_programs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.DropColumn(
                name: "Id",
                table: "organizations_categories");
            
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "organizations_categories",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "organizations_categories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Endpoint",
                table: "organizations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentName",
                table: "organizations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "organizations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "TransportId",
                table: "organizations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastVisit",
                table: "customers",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddPrimaryKey(
                name: "PK_organizations_members",
                table: "organizations_members",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_organizations_categories",
                table: "organizations_categories",
                column: "Id");
            
            migrationBuilder.AddPrimaryKey(
                name: "PK_organizations_programs",
                table: "organizations_programs",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "customers_categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers_categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_customers_categories_customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_organizations_members_OrganizationId",
                table: "organizations_members",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_organizations_categories_OrganizationId",
                table: "organizations_categories",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_customer_categories_CustomerId",
                table: "customers_categories",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customers_categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_organizations_members",
                table: "organizations_members");

            migrationBuilder.DropIndex(
                name: "IX_organizations_members_OrganizationId",
                table: "organizations_members");

            migrationBuilder.DropPrimaryKey(
                name: "PK_organizations_categories",
                table: "organizations_categories");

            migrationBuilder.DropIndex(
                name: "IX_organizations_categories_OrganizationId",
                table: "organizations_categories");

            migrationBuilder.DropColumn(
                name: "ProgramId",
                table: "organizations_programs");

            migrationBuilder.DropColumn(
                name: "WalletId",
                table: "organizations_programs");

            migrationBuilder.DropColumn(
                name: "WalletType",
                table: "organizations_programs");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "organizations_categories");

            migrationBuilder.DropColumn(
                name: "Endpoint",
                table: "organizations");

            migrationBuilder.DropColumn(
                name: "PaymentName",
                table: "organizations");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "organizations");

            migrationBuilder.DropColumn(
                name: "TransportId",
                table: "organizations");

            migrationBuilder.DropColumn(
                name: "LastVisit",
                table: "customers");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "organizations_programs",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "organizations_categories",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_organizations_members",
                table: "organizations_members",
                columns: new[] { "OrganizationId", "Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_organizations_categories",
                table: "organizations_categories",
                columns: new[] { "OrganizationId", "Id" });

            migrationBuilder.CreateTable(
                name: "customers_wallets",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Balance = table.Column<double>(type: "float", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProgramType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WalletId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers_wallets", x => new { x.CustomerId, x.Id });
                    table.ForeignKey(
                        name: "FK_customers_wallets_customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "organizations_programs_wallets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProgramId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProgramType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organizations_programs_wallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_organizations_programs_wallets_organizations_programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "organizations_programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_organizations_programs_wallets_ProgramId",
                table: "organizations_programs_wallets",
                column: "ProgramId");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OzonCard.Context.Migrations
{
    public partial class timeUpdateBalance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Update",
                table: "CustomerWallets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 8, 1, 10, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Update",
                table: "CustomerWallets");
        }
    }
}

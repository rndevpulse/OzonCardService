using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OzonCard.Context.Migrations
{
    public partial class events2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MarketingCampaignName",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProgramName",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarketingCampaignName",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ProgramName",
                table: "Events");
        }
    }
}

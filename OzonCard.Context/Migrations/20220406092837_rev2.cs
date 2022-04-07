using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OzonCard.Context.Migrations
{
    public partial class rev2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerWallets_Wallets_WalletId",
                table: "CustomerWallets");

            migrationBuilder.DropIndex(
                name: "IX_CustomerWallets_WalletId",
                table: "CustomerWallets");

            migrationBuilder.AlterColumn<Guid>(
                name: "WalletId",
                table: "CustomerWallets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "WalletId",
                table: "CustomerWallets",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerWallets_WalletId",
                table: "CustomerWallets",
                column: "WalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerWallets_Wallets_WalletId",
                table: "CustomerWallets",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id");
        }
    }
}

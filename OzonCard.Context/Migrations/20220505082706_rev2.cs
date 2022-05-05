using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OzonCard.Context.Migrations
{
    public partial class rev2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryCustomer_Categories_CategoriesId",
                table: "CategoryCustomer");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryCustomer_Customers_CustomersId",
                table: "CategoryCustomer");

            migrationBuilder.RenameColumn(
                name: "CustomersId",
                table: "CategoryCustomer",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "CategoriesId",
                table: "CategoryCustomer",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryCustomer_CustomersId",
                table: "CategoryCustomer",
                newName: "IX_CategoryCustomer_CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryCustomer_Categories_CategoryId",
                table: "CategoryCustomer",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryCustomer_Customers_CustomerId",
                table: "CategoryCustomer",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryCustomer_Categories_CategoryId",
                table: "CategoryCustomer");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryCustomer_Customers_CustomerId",
                table: "CategoryCustomer");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "CategoryCustomer",
                newName: "CustomersId");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "CategoryCustomer",
                newName: "CategoriesId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryCustomer_CategoryId",
                table: "CategoryCustomer",
                newName: "IX_CategoryCustomer_CustomersId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryCustomer_Categories_CategoriesId",
                table: "CategoryCustomer",
                column: "CategoriesId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryCustomer_Customers_CustomersId",
                table: "CategoryCustomer",
                column: "CustomersId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

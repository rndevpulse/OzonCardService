using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OzonCard.Common.Infrastructure.Database.Migrations.Security
{
    /// <inheritdoc />
    public partial class UserSecurityMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "security",
                table: "role",
                keyColumn: "Id",
                keyValue: "21d938a6-66e3-4aad-abbc-9a176e1ae506",
                column: "ConcurrencyStamp",
                value: "b6fc9ed8231f402a93ac8944602d1146");

            migrationBuilder.UpdateData(
                schema: "security",
                table: "role",
                keyColumn: "Id",
                keyValue: "44064c53-4cd3-472c-9895-eabf9464dc2d",
                column: "ConcurrencyStamp",
                value: "927a39965d6e4520a67dd8a4182b2d35");

            migrationBuilder.UpdateData(
                schema: "security",
                table: "role",
                keyColumn: "Id",
                keyValue: "e88fbc30-1985-4379-ae4c-e24657835212",
                column: "ConcurrencyStamp",
                value: "9fd89f070e2646e0b791295ee8abe6a7");

            migrationBuilder.InsertData(
                schema: "security",
                table: "user",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "4cc34a7e-3c7f-4b92-9279-b7b2def75fff", 0, "cf3b4531-7b1a-4382-bba4-c39e548b5ad4", "kolur_20@bk.ru", true, false, null, "KOLUR_20@BK.RU", "KOLUR_20@BK.RU", "AQAAAAIAAYagAAAAEJJrBy0pq8RJrN8I6i77dfBhHuSE9ulo98YLCchlOwDExKR2BNk6swyNIIOrFtOBPA==", null, false, "XJ5DBWJXEXC4HELI3LEARREZ6VDY4YBS", false, "kolur_20@bk.ru" });

            migrationBuilder.InsertData(
                schema: "security",
                table: "user_roles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "21d938a6-66e3-4aad-abbc-9a176e1ae506", "4cc34a7e-3c7f-4b92-9279-b7b2def75fff" },
                    { "44064c53-4cd3-472c-9895-eabf9464dc2d", "4cc34a7e-3c7f-4b92-9279-b7b2def75fff" },
                    { "e88fbc30-1985-4379-ae4c-e24657835212", "4cc34a7e-3c7f-4b92-9279-b7b2def75fff" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "security",
                table: "user_roles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "21d938a6-66e3-4aad-abbc-9a176e1ae506", "4cc34a7e-3c7f-4b92-9279-b7b2def75fff" });

            migrationBuilder.DeleteData(
                schema: "security",
                table: "user_roles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "44064c53-4cd3-472c-9895-eabf9464dc2d", "4cc34a7e-3c7f-4b92-9279-b7b2def75fff" });

            migrationBuilder.DeleteData(
                schema: "security",
                table: "user_roles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "e88fbc30-1985-4379-ae4c-e24657835212", "4cc34a7e-3c7f-4b92-9279-b7b2def75fff" });

            migrationBuilder.DeleteData(
                schema: "security",
                table: "user",
                keyColumn: "Id",
                keyValue: "4cc34a7e-3c7f-4b92-9279-b7b2def75fff");

            migrationBuilder.UpdateData(
                schema: "security",
                table: "role",
                keyColumn: "Id",
                keyValue: "21d938a6-66e3-4aad-abbc-9a176e1ae506",
                column: "ConcurrencyStamp",
                value: "83b1a412112c4624b64ddea65b648dd8");

            migrationBuilder.UpdateData(
                schema: "security",
                table: "role",
                keyColumn: "Id",
                keyValue: "44064c53-4cd3-472c-9895-eabf9464dc2d",
                column: "ConcurrencyStamp",
                value: "b5f3d8a826684ef9805453aedb86f98b");

            migrationBuilder.UpdateData(
                schema: "security",
                table: "role",
                keyColumn: "Id",
                keyValue: "e88fbc30-1985-4379-ae4c-e24657835212",
                column: "ConcurrencyStamp",
                value: "c129723cdf314dc199af03d74cb60ef3");
        }
    }
}

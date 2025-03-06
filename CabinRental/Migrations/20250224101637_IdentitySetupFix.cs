using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CabinRental.Migrations
{
    /// <inheritdoc />
    public partial class IdentitySetupFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cabins",
                keyColumn: "Id",
                keyValue: 1,
                column: "Price",
                value: 100.0);

            migrationBuilder.UpdateData(
                table: "Cabins",
                keyColumn: "Id",
                keyValue: 2,
                column: "Price",
                value: 200.0);

            migrationBuilder.UpdateData(
                table: "Cabins",
                keyColumn: "Id",
                keyValue: 3,
                column: "Price",
                value: 300.0);

            migrationBuilder.UpdateData(
                table: "Cabins",
                keyColumn: "Id",
                keyValue: 4,
                column: "Price",
                value: 400.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cabins",
                keyColumn: "Id",
                keyValue: 1,
                column: "Price",
                value: 1000.0);

            migrationBuilder.UpdateData(
                table: "Cabins",
                keyColumn: "Id",
                keyValue: 2,
                column: "Price",
                value: 2000.0);

            migrationBuilder.UpdateData(
                table: "Cabins",
                keyColumn: "Id",
                keyValue: 3,
                column: "Price",
                value: 3000.0);

            migrationBuilder.UpdateData(
                table: "Cabins",
                keyColumn: "Id",
                keyValue: 4,
                column: "Price",
                value: 4000.0);
        }
    }
}

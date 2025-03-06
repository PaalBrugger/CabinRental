using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CabinRental.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CabinImages",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImagePath",
                value: "/Images/Cabin/Cabin1.webp");

            migrationBuilder.UpdateData(
                table: "CabinImages",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImagePath",
                value: "/Images/Cabin/Cabin1Interior.webp");

            migrationBuilder.UpdateData(
                table: "CabinImages",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImagePath",
                value: "/Images/Cabin/Cabin2.webp");

            migrationBuilder.UpdateData(
                table: "CabinImages",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImagePath",
                value: "/Images/Cabin/Cabin2Interior.webp");

            migrationBuilder.UpdateData(
                table: "CabinImages",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImagePath",
                value: "/Images/Cabin/Cabin3.webp");

            migrationBuilder.UpdateData(
                table: "CabinImages",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImagePath",
                value: "/Images/Cabin/Cabin3Interior.webp");

            migrationBuilder.UpdateData(
                table: "CabinImages",
                keyColumn: "Id",
                keyValue: 7,
                column: "ImagePath",
                value: "/Images/Cabin/Cabin4.webp");

            migrationBuilder.UpdateData(
                table: "CabinImages",
                keyColumn: "Id",
                keyValue: 8,
                column: "ImagePath",
                value: "/Images/Cabin/Cabin4Interior.webp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CabinImages",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImagePath",
                value: "Images/Cabin/Cabin1.webp");

            migrationBuilder.UpdateData(
                table: "CabinImages",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImagePath",
                value: "Images/Cabin/Cabin1Interior.webp");

            migrationBuilder.UpdateData(
                table: "CabinImages",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImagePath",
                value: "Images/Cabin/Cabin2.webp");

            migrationBuilder.UpdateData(
                table: "CabinImages",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImagePath",
                value: "Images/Cabin/Cabin2Interior.webp");

            migrationBuilder.UpdateData(
                table: "CabinImages",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImagePath",
                value: "Images/Cabin/Cabin3.webp");

            migrationBuilder.UpdateData(
                table: "CabinImages",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImagePath",
                value: "Images/Cabin/Cabin3Interior.webp");

            migrationBuilder.UpdateData(
                table: "CabinImages",
                keyColumn: "Id",
                keyValue: 7,
                column: "ImagePath",
                value: "Images/Cabin/Cabin4.webp");

            migrationBuilder.UpdateData(
                table: "CabinImages",
                keyColumn: "Id",
                keyValue: 8,
                column: "ImagePath",
                value: "Images/Cabin/Cabin4Interior.webp");
        }
    }
}

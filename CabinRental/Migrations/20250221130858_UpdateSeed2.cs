using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CabinRental.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeed2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cabins",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Surrounded by snow-draped pines, this cozy retreat offers a crackling fireplace, stunning views, and pure serenity. 🌨️🔥");

            migrationBuilder.UpdateData(
                table: "Cabins",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "Experience the magic of winter in this rustic log cabin. Snuggle up with a warm drink and watch the snow fall outside. ☕❄️");

            migrationBuilder.UpdateData(
                table: "Cabins",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "Tucked away in a snowy forest, this charming cabin is the perfect retreat for a peaceful and cozy getaway. 🌲🏡");

            migrationBuilder.UpdateData(
                table: "Cabins",
                keyColumn: "Id",
                keyValue: 4,
                column: "Description",
                value: "Escape to a winter wonderland in this cozy, snow-covered cabin. Warm up by the fireplace and enjoy breathtaking mountain views. ❄️🔥");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cabins",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Cabins",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Cabins",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Cabins",
                keyColumn: "Id",
                keyValue: 4,
                column: "Description",
                value: null);
        }
    }
}

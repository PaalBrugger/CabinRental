using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CabinRental.Migrations
{
    /// <inheritdoc />
    public partial class SeedCabinImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Cabins");

            migrationBuilder.CreateTable(
                name: "CabinImage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CabinId = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CabinImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CabinImage_Cabins_CabinId",
                        column: x => x.CabinId,
                        principalTable: "Cabins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CabinImage",
                columns: new[] { "Id", "CabinId", "ImagePath" },
                values: new object[,]
                {
                    { 1, 1, "Images/Cabin/Cabin1.webp" },
                    { 2, 1, "Images/Cabin/Cabin1Interior.webp" },
                    { 3, 2, "Images/Cabin/Cabin2.webp" },
                    { 4, 2, "Images/Cabin/Cabin2Interior.webp" },
                    { 5, 3, "Images/Cabin/Cabin3.webp" },
                    { 6, 3, "Images/Cabin/Cabin3Interior.webp" },
                    { 7, 4, "Images/Cabin/Cabin4.webp" },
                    { 8, 4, "Images/Cabin/Cabin4Interior.webp" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CabinImage_CabinId",
                table: "CabinImage",
                column: "CabinId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CabinImage");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Cabins",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Cabins",
                keyColumn: "Id",
                keyValue: 1,
                column: "Image",
                value: "Lake Cabin.jpg");

            migrationBuilder.UpdateData(
                table: "Cabins",
                keyColumn: "Id",
                keyValue: 2,
                column: "Image",
                value: "Lake Cabin.jpg");

            migrationBuilder.UpdateData(
                table: "Cabins",
                keyColumn: "Id",
                keyValue: 3,
                column: "Image",
                value: "Lake Cabin.jpg");

            migrationBuilder.UpdateData(
                table: "Cabins",
                keyColumn: "Id",
                keyValue: 4,
                column: "Image",
                value: "Lake Cabin.jpg");
        }
    }
}

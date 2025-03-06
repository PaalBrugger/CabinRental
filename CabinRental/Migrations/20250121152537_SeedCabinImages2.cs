using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CabinRental.Migrations
{
    /// <inheritdoc />
    public partial class SeedCabinImages2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CabinImage_Cabins_CabinId",
                table: "CabinImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CabinImage",
                table: "CabinImage");

            migrationBuilder.RenameTable(
                name: "CabinImage",
                newName: "CabinImages");

            migrationBuilder.RenameIndex(
                name: "IX_CabinImage_CabinId",
                table: "CabinImages",
                newName: "IX_CabinImages_CabinId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CabinImages",
                table: "CabinImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CabinImages_Cabins_CabinId",
                table: "CabinImages",
                column: "CabinId",
                principalTable: "Cabins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CabinImages_Cabins_CabinId",
                table: "CabinImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CabinImages",
                table: "CabinImages");

            migrationBuilder.RenameTable(
                name: "CabinImages",
                newName: "CabinImage");

            migrationBuilder.RenameIndex(
                name: "IX_CabinImages_CabinId",
                table: "CabinImage",
                newName: "IX_CabinImage_CabinId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CabinImage",
                table: "CabinImage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CabinImage_Cabins_CabinId",
                table: "CabinImage",
                column: "CabinId",
                principalTable: "Cabins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

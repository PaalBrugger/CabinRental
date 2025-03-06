using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CabinRental.Migrations
{
    /// <inheritdoc />
    public partial class FixTypoReservationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Adress",
                table: "Reservations",
                newName: "Address");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Reservations",
                newName: "Adress");
        }
    }
}

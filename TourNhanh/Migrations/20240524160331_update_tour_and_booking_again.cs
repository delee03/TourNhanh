using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourNhanh.Migrations
{
    /// <inheritdoc />
    public partial class update_tour_and_booking_again : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RemainingSlots",
                table: "Tours",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemainingSlots",
                table: "Tours");
        }
    }
}

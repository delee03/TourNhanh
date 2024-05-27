using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourNhanh.Migrations
{
    /// <inheritdoc />
    public partial class Update_Booking_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isPaymentCompleted",
                table: "Bookings",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isPaymentCompleted",
                table: "Bookings");
        }
    }
}

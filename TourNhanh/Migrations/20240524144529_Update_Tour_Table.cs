using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourNhanh.Migrations
{
    /// <inheritdoc />
    public partial class Update_Tour_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "maxParticipants",
                table: "Tours",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "maxParticipants",
                table: "Tours");
        }
    }
}

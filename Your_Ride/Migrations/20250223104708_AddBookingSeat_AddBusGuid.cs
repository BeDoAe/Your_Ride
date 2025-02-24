using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Your_Ride.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingSeat_AddBusGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Seats",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SeatId",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BusGuideId",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "HasCompleted",
                table: "Appointments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Books_SeatId",
                table: "Books",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_BusGuideId",
                table: "Appointments",
                column: "BusGuideId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AspNetUsers_BusGuideId",
                table: "Appointments",
                column: "BusGuideId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Seats_SeatId",
                table: "Books",
                column: "SeatId",
                principalTable: "Seats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AspNetUsers_BusGuideId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Seats_SeatId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_SeatId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_BusGuideId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "SeatId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BusGuideId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "HasCompleted",
                table: "Appointments");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Your_Ride.Migrations
{
    /// <inheritdoc />
    public partial class modifyTime_AppointmentModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AspNetUsers_BusGuideId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_BusGuideId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "BusGuideId",
                table: "Appointments");

            migrationBuilder.AddColumn<string>(
                name: "BusGuideId",
                table: "Times",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Times_BusGuideId",
                table: "Times",
                column: "BusGuideId");

            migrationBuilder.AddForeignKey(
                name: "FK_Times_AspNetUsers_BusGuideId",
                table: "Times",
                column: "BusGuideId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Times_AspNetUsers_BusGuideId",
                table: "Times");

            migrationBuilder.DropIndex(
                name: "IX_Times_BusGuideId",
                table: "Times");

            migrationBuilder.DropColumn(
                name: "BusGuideId",
                table: "Times");

            migrationBuilder.AddColumn<string>(
                name: "BusGuideId",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

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
        }
    }
}

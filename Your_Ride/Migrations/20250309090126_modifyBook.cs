using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Your_Ride.Migrations
{
    /// <inheritdoc />
    public partial class modifyBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Appointments_AppointmentId",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "AppointmentId",
                table: "Books",
                newName: "timeId");

            migrationBuilder.RenameIndex(
                name: "IX_Books_AppointmentId",
                table: "Books",
                newName: "IX_Books_timeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Times_timeId",
                table: "Books",
                column: "timeId",
                principalTable: "Times",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Times_timeId",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "timeId",
                table: "Books",
                newName: "AppointmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Books_timeId",
                table: "Books",
                newName: "IX_Books_AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Appointments_AppointmentId",
                table: "Books",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

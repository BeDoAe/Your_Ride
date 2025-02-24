using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Your_Ride.Migrations
{
    /// <inheritdoc />
    public partial class RelationsOfBook_Appointment_Time : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Books_BookId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_AspNetUsers_UserID",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_BookId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "MaxAmount",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Appointments");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "Books",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "AppointmentId",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "MaxAmount",
                table: "Appointments",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_Books_AppointmentId",
                table: "Books",
                column: "AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Appointments_AppointmentId",
                table: "Books",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_AspNetUsers_UserID",
                table: "Books",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Appointments_AppointmentId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_AspNetUsers_UserID",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_AppointmentId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "MaxAmount",
                table: "Appointments");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "Books",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MaxAmount",
                table: "Books",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_BookId",
                table: "Appointments",
                column: "BookId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Books_BookId",
                table: "Appointments",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_AspNetUsers_UserID",
                table: "Books",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

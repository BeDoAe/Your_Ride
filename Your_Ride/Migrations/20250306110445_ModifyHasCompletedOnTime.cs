using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Your_Ride.Migrations
{
    /// <inheritdoc />
    public partial class ModifyHasCompletedOnTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTransactionLog_Appointments_AppointmentId",
                table: "UserTransactionLog");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTransactionLog_AspNetUsers_UserId",
                table: "UserTransactionLog");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTransactionLog_Times_TimeId",
                table: "UserTransactionLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTransactionLog",
                table: "UserTransactionLog");

            migrationBuilder.DropColumn(
                name: "HasCompleted",
                table: "Appointments");

            migrationBuilder.RenameTable(
                name: "UserTransactionLog",
                newName: "userTransactionLogs");

            migrationBuilder.RenameIndex(
                name: "IX_UserTransactionLog_UserId",
                table: "userTransactionLogs",
                newName: "IX_userTransactionLogs_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserTransactionLog_TimeId",
                table: "userTransactionLogs",
                newName: "IX_userTransactionLogs_TimeId");

            migrationBuilder.RenameIndex(
                name: "IX_UserTransactionLog_AppointmentId",
                table: "userTransactionLogs",
                newName: "IX_userTransactionLogs_AppointmentId");

            migrationBuilder.AddColumn<bool>(
                name: "HasCompleted",
                table: "Times",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_userTransactionLogs",
                table: "userTransactionLogs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_userTransactionLogs_Appointments_AppointmentId",
                table: "userTransactionLogs",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_userTransactionLogs_AspNetUsers_UserId",
                table: "userTransactionLogs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_userTransactionLogs_Times_TimeId",
                table: "userTransactionLogs",
                column: "TimeId",
                principalTable: "Times",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userTransactionLogs_Appointments_AppointmentId",
                table: "userTransactionLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_userTransactionLogs_AspNetUsers_UserId",
                table: "userTransactionLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_userTransactionLogs_Times_TimeId",
                table: "userTransactionLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_userTransactionLogs",
                table: "userTransactionLogs");

            migrationBuilder.DropColumn(
                name: "HasCompleted",
                table: "Times");

            migrationBuilder.RenameTable(
                name: "userTransactionLogs",
                newName: "UserTransactionLog");

            migrationBuilder.RenameIndex(
                name: "IX_userTransactionLogs_UserId",
                table: "UserTransactionLog",
                newName: "IX_UserTransactionLog_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_userTransactionLogs_TimeId",
                table: "UserTransactionLog",
                newName: "IX_UserTransactionLog_TimeId");

            migrationBuilder.RenameIndex(
                name: "IX_userTransactionLogs_AppointmentId",
                table: "UserTransactionLog",
                newName: "IX_UserTransactionLog_AppointmentId");

            migrationBuilder.AddColumn<bool>(
                name: "HasCompleted",
                table: "Appointments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTransactionLog",
                table: "UserTransactionLog",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTransactionLog_Appointments_AppointmentId",
                table: "UserTransactionLog",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTransactionLog_AspNetUsers_UserId",
                table: "UserTransactionLog",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTransactionLog_Times_TimeId",
                table: "UserTransactionLog",
                column: "TimeId",
                principalTable: "Times",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Your_Ride.Migrations
{
    /// <inheritdoc />
    public partial class IsDeletedLocationPic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationImage_Times_TimeId",
                table: "LocationImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LocationImage",
                table: "LocationImage");

            migrationBuilder.RenameTable(
                name: "LocationImage",
                newName: "LocationImages");

            migrationBuilder.RenameIndex(
                name: "IX_LocationImage_TimeId",
                table: "LocationImages",
                newName: "IX_LocationImages_TimeId");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "LocationImages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LocationImages",
                table: "LocationImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationImages_Times_TimeId",
                table: "LocationImages",
                column: "TimeId",
                principalTable: "Times",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationImages_Times_TimeId",
                table: "LocationImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LocationImages",
                table: "LocationImages");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "LocationImages");

            migrationBuilder.RenameTable(
                name: "LocationImages",
                newName: "LocationImage");

            migrationBuilder.RenameIndex(
                name: "IX_LocationImages_TimeId",
                table: "LocationImage",
                newName: "IX_LocationImage_TimeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LocationImage",
                table: "LocationImage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationImage_Times_TimeId",
                table: "LocationImage",
                column: "TimeId",
                principalTable: "Times",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

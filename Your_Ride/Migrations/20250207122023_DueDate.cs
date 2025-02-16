using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Your_Ride.Migrations
{
    /// <inheritdoc />
    public partial class DueDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DueDateArrivalSubmission",
                table: "Times",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDateDepartureSubmission",
                table: "Times",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueDateArrivalSubmission",
                table: "Times");

            migrationBuilder.DropColumn(
                name: "DueDateDepartureSubmission",
                table: "Times");
        }
    }
}

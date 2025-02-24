using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Your_Ride.Migrations
{
    /// <inheritdoc />
    public partial class columnfee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Fee",
                table: "Times",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fee",
                table: "Times");
        }
    }
}

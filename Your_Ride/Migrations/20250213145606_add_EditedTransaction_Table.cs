using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Your_Ride.Migrations
{
    /// <inheritdoc />
    public partial class add_EditedTransaction_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EditedTransactionDate",
                table: "Transactions",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EditedTransactionDate",
                table: "Transactions");
        }
    }
}

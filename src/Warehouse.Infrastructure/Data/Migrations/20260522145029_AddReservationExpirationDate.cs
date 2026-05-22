using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReservationExpirationDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "expires_at",
                table: "stock_reservations",
                "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW() + (random() * interval '30 days' - interval '15 days')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "expires_at",
                table: "stock_reservations");
        }
    }
}
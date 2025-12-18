using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ezyGo.Payment.Storage.Migrations
{
    /// <inheritdoc />
    public partial class addTicketNamesParms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SeatNames",
                table: "PaymentInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeatNames",
                table: "PaymentInfos");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ezyGo.Admin.Storage.Migrations
{
    /// <inheritdoc />
    public partial class updatestationroutestoppagerelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RouteStoppages_BusStations_BusStationEntityId",
                table: "RouteStoppages");

            migrationBuilder.AddForeignKey(
                name: "FK_RouteStoppages_BusStations_BusStationEntityId",
                table: "RouteStoppages",
                column: "BusStationEntityId",
                principalTable: "BusStations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RouteStoppages_BusStations_BusStationEntityId",
                table: "RouteStoppages");

            migrationBuilder.AddForeignKey(
                name: "FK_RouteStoppages_BusStations_BusStationEntityId",
                table: "RouteStoppages",
                column: "BusStationEntityId",
                principalTable: "BusStations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

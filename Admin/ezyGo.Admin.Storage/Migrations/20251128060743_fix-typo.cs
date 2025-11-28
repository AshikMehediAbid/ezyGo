using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ezyGo.Admin.Storage.Migrations
{
    /// <inheritdoc />
    public partial class fixtypo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_BusStations_EndingingPointId",
                table: "Routes");

            migrationBuilder.RenameColumn(
                name: "EndingingPointId",
                table: "Routes",
                newName: "EndingPointId");

            migrationBuilder.RenameIndex(
                name: "IX_Routes_EndingingPointId",
                table: "Routes",
                newName: "IX_Routes_EndingPointId");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_BusStations_EndingPointId",
                table: "Routes",
                column: "EndingPointId",
                principalTable: "BusStations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_BusStations_EndingPointId",
                table: "Routes");

            migrationBuilder.RenameColumn(
                name: "EndingPointId",
                table: "Routes",
                newName: "EndingingPointId");

            migrationBuilder.RenameIndex(
                name: "IX_Routes_EndingPointId",
                table: "Routes",
                newName: "IX_Routes_EndingingPointId");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_BusStations_EndingingPointId",
                table: "Routes",
                column: "EndingingPointId",
                principalTable: "BusStations",
                principalColumn: "Id");
        }
    }
}

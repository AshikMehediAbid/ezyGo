using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ezyGo.Admin.Storage.Migrations
{
    /// <inheritdoc />
    public partial class addRoutestoppagetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Geo_Longitude",
                table: "BusStations",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Geo_Latitude",
                table: "BusStations",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartingPointId = table.Column<int>(type: "int", nullable: true),
                    EndingingPointId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Routes_BusStations_EndingingPointId",
                        column: x => x.EndingingPointId,
                        principalTable: "BusStations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Routes_BusStations_StartingPointId",
                        column: x => x.StartingPointId,
                        principalTable: "BusStations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RouteStoppages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteEntityId = table.Column<int>(type: "int", nullable: false),
                    BusStationEntityId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteStoppages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteStoppages_BusStations_BusStationEntityId",
                        column: x => x.BusStationEntityId,
                        principalTable: "BusStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RouteStoppages_Routes_RouteEntityId",
                        column: x => x.RouteEntityId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Routes_EndingingPointId",
                table: "Routes",
                column: "EndingingPointId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_StartingPointId",
                table: "Routes",
                column: "StartingPointId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteStoppages_BusStationEntityId",
                table: "RouteStoppages",
                column: "BusStationEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteStoppages_RouteEntityId",
                table: "RouteStoppages",
                column: "RouteEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RouteStoppages");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.AlterColumn<decimal>(
                name: "Geo_Longitude",
                table: "BusStations",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Geo_Latitude",
                table: "BusStations",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }
    }
}

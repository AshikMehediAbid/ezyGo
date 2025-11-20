using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ezyGo.Admin.Storage.Migrations
{
    /// <inheritdoc />
    public partial class CreateTrainStationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainStations",
                columns: table => new
                {
                    TrainStationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainStationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrainStationDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Geo_Latitude = table.Column<int>(type: "int", nullable: false),
                    Geo_Longitude = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainStations", x => x.TrainStationId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainStations");
        }
    }
}

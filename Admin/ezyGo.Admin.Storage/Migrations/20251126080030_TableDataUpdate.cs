using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ezyGo.Admin.Storage.Migrations
{
    /// <inheritdoc />
    public partial class TableDataUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bus");

            migrationBuilder.CreateTable(
                name: "Buses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BusRegNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalCapacity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BusType = table.Column<int>(type: "int", nullable: false),
                    DriverName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Buses_BusCompanies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "BusCompanies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Buses_CompanyId",
                table: "Buses",
                column: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Buses");

            migrationBuilder.CreateTable(
                name: "Bus",
                columns: table => new
                {
                    BusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusCompanyEntityCompanyId = table.Column<int>(type: "int", nullable: true),
                    BusName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BusRegNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    DriverName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bus", x => x.BusId);
                    table.ForeignKey(
                        name: "FK_Bus_BusCompanies_BusCompanyEntityCompanyId",
                        column: x => x.BusCompanyEntityCompanyId,
                        principalTable: "BusCompanies",
                        principalColumn: "CompanyId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bus_BusCompanyEntityCompanyId",
                table: "Bus",
                column: "BusCompanyEntityCompanyId");
        }
    }
}

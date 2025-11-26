using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ezyGo.Admin.Storage.Migrations
{
    /// <inheritdoc />
    public partial class BusBusCompanyRelationShip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buses_BusCompanies_CompanyId",
                table: "Buses");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Buses",
                newName: "BusCompanyEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_Buses_CompanyId",
                table: "Buses",
                newName: "IX_Buses_BusCompanyEntityId");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "BusCompanies",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Buses_BusCompanies_BusCompanyEntityId",
                table: "Buses",
                column: "BusCompanyEntityId",
                principalTable: "BusCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buses_BusCompanies_BusCompanyEntityId",
                table: "Buses");

            migrationBuilder.RenameColumn(
                name: "BusCompanyEntityId",
                table: "Buses",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Buses_BusCompanyEntityId",
                table: "Buses",
                newName: "IX_Buses_CompanyId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "BusCompanies",
                newName: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Buses_BusCompanies_CompanyId",
                table: "Buses",
                column: "CompanyId",
                principalTable: "BusCompanies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ezyGo.Admin.Storage.Migrations
{
    /// <inheritdoc />
    public partial class buscompanyRelationUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buses_BusCompanies_BusCompanyEntityId",
                table: "Buses");

            migrationBuilder.AlterColumn<int>(
                name: "BusCompanyEntityId",
                table: "Buses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Buses_BusCompanies_BusCompanyEntityId",
                table: "Buses",
                column: "BusCompanyEntityId",
                principalTable: "BusCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buses_BusCompanies_BusCompanyEntityId",
                table: "Buses");

            migrationBuilder.AlterColumn<int>(
                name: "BusCompanyEntityId",
                table: "Buses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Buses_BusCompanies_BusCompanyEntityId",
                table: "Buses",
                column: "BusCompanyEntityId",
                principalTable: "BusCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

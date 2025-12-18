using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ezyGo.Payment.Storage.Migrations
{
    /// <inheritdoc />
    public partial class modifyPaymentInfoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Seats",
                table: "PaymentInfos",
                newName: "To");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "PaymentInfos",
                newName: "Fare");

            migrationBuilder.AddColumn<string>(
                name: "BusNumber",
                table: "PaymentInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "From",
                table: "PaymentInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "JourneyDate",
                table: "PaymentInfos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PassengerEmail",
                table: "PaymentInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PassengerName",
                table: "PaymentInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PassengerPhone",
                table: "PaymentInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SeatNumbers",
                table: "PaymentInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusNumber",
                table: "PaymentInfos");

            migrationBuilder.DropColumn(
                name: "From",
                table: "PaymentInfos");

            migrationBuilder.DropColumn(
                name: "JourneyDate",
                table: "PaymentInfos");

            migrationBuilder.DropColumn(
                name: "PassengerEmail",
                table: "PaymentInfos");

            migrationBuilder.DropColumn(
                name: "PassengerName",
                table: "PaymentInfos");

            migrationBuilder.DropColumn(
                name: "PassengerPhone",
                table: "PaymentInfos");

            migrationBuilder.DropColumn(
                name: "SeatNumbers",
                table: "PaymentInfos");

            migrationBuilder.RenameColumn(
                name: "To",
                table: "PaymentInfos",
                newName: "Seats");

            migrationBuilder.RenameColumn(
                name: "Fare",
                table: "PaymentInfos",
                newName: "Amount");
        }
    }
}

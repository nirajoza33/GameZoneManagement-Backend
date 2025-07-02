using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameZoneManagementApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTblBookingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppliedOfferId",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "Bookings",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasOfferApplied",
                table: "Bookings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OfferCode",
                table: "Bookings",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OriginalPrice",
                table: "Bookings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Bookings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_AppliedOfferId",
                table: "Bookings",
                column: "AppliedOfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_TblOffers_AppliedOfferId",
                table: "Bookings",
                column: "AppliedOfferId",
                principalTable: "TblOffers",
                principalColumn: "OfferId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_TblOffers_AppliedOfferId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_AppliedOfferId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "AppliedOfferId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "HasOfferApplied",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "OfferCode",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "OriginalPrice",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Bookings");
        }
    }
}

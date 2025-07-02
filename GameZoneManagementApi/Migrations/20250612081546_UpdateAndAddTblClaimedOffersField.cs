using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameZoneManagementApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAndAddTblClaimedOffersField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ActualDiscountApplied",
                table: "TblClaimedOffer",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "BookingId",
                table: "TblClaimedOffer",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TblClaimedOffer_BookingId",
                table: "TblClaimedOffer",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_TblClaimedOffer_Bookings_BookingId",
                table: "TblClaimedOffer",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "BookingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblClaimedOffer_Bookings_BookingId",
                table: "TblClaimedOffer");

            migrationBuilder.DropIndex(
                name: "IX_TblClaimedOffer_BookingId",
                table: "TblClaimedOffer");

            migrationBuilder.DropColumn(
                name: "ActualDiscountApplied",
                table: "TblClaimedOffer");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "TblClaimedOffer");
        }
    }
}

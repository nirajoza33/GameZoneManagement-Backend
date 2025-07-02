using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameZoneManagementApi.Migrations
{
    /// <inheritdoc />
    public partial class AddTblClaimOffer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblOffers_Tblusers_UserId",
                table: "TblOffers");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "TblOffers",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsTrending",
                table: "TblOffers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsFeatured",
                table: "TblOffers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "TblOffers",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "TblClaimedOffer",
                columns: table => new
                {
                    ClaimedOfferId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfferId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    OfferCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblClaimedOffer", x => x.ClaimedOfferId);
                    table.ForeignKey(
                        name: "FK_TblClaimedOffer_TblOffers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "TblOffers",
                        principalColumn: "OfferId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TblClaimedOffer_Tblusers_UserId",
                        column: x => x.UserId,
                        principalTable: "Tblusers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TblOffers_Category",
                table: "TblOffers",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_TblOffers_Status",
                table: "TblOffers",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TblOffers_ValidUntil",
                table: "TblOffers",
                column: "ValidUntil");

            migrationBuilder.CreateIndex(
                name: "IX_TblClaimedOffer_OfferId",
                table: "TblClaimedOffer",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_TblClaimedOffer_UserId",
                table: "TblClaimedOffer",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TblOffers_Tblusers_UserId",
                table: "TblOffers",
                column: "UserId",
                principalTable: "Tblusers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblOffers_Tblusers_UserId",
                table: "TblOffers");

            migrationBuilder.DropTable(
                name: "TblClaimedOffer");

            migrationBuilder.DropIndex(
                name: "IX_TblOffers_Category",
                table: "TblOffers");

            migrationBuilder.DropIndex(
                name: "IX_TblOffers_Status",
                table: "TblOffers");

            migrationBuilder.DropIndex(
                name: "IX_TblOffers_ValidUntil",
                table: "TblOffers");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "TblOffers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsTrending",
                table: "TblOffers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsFeatured",
                table: "TblOffers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "TblOffers",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AddForeignKey(
                name: "FK_TblOffers_Tblusers_UserId",
                table: "TblOffers",
                column: "UserId",
                principalTable: "Tblusers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

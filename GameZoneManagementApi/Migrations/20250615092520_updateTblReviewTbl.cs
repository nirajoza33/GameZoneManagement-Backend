using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameZoneManagementApi.Migrations
{
    /// <inheritdoc />
    public partial class updateTblReviewTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrls",
                table: "TblReviews",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrls",
                table: "TblReviews");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameZoneManagementApi.Migrations
{
    /// <inheritdoc />
    public partial class addTblRepliesAndLikes_UpdateTblReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "TblReviews",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "ReviewText",
                table: "TblReviews",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Replies",
                table: "TblReviews",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Likes",
                table: "TblReviews",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "TblReviews",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TblReviews",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "TblReviews",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TblReviewLikes",
                columns: table => new
                {
                    LikeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReviewId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblReviewLikes", x => x.LikeId);
                    table.ForeignKey(
                        name: "FK_TblReviewLikes_TblReviews_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "TblReviews",
                        principalColumn: "ReviewId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TblReviewLikes_Tblusers_UserId",
                        column: x => x.UserId,
                        principalTable: "Tblusers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TblReviewReplies",
                columns: table => new
                {
                    ReplyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReviewId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ReplyText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblReviewReplies", x => x.ReplyId);
                    table.ForeignKey(
                        name: "FK_TblReviewReplies_TblReviews_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "TblReviews",
                        principalColumn: "ReviewId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TblReviewReplies_Tblusers_UserId",
                        column: x => x.UserId,
                        principalTable: "Tblusers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TblReviewLikes_CreatedDate",
                table: "TblReviewLikes",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_TblReviewLikes_ReviewId",
                table: "TblReviewLikes",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_TblReviewLikes_ReviewId_UserId",
                table: "TblReviewLikes",
                columns: new[] { "ReviewId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TblReviewLikes_UserId",
                table: "TblReviewLikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TblReviewReplies_CreatedDate",
                table: "TblReviewReplies",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_TblReviewReplies_ReviewId",
                table: "TblReviewReplies",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_TblReviewReplies_UserId",
                table: "TblReviewReplies",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblReviewLikes");

            migrationBuilder.DropTable(
                name: "TblReviewReplies");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TblReviews");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "TblReviews");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "TblReviews",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "ReviewText",
                table: "TblReviews",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<int>(
                name: "Replies",
                table: "TblReviews",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Likes",
                table: "TblReviews",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "TblReviews",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");
        }
    }
}

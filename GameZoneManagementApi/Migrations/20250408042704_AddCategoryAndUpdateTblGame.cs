using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameZoneManagementApi.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryAndUpdateTblGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_GameZoneOwners_GameZoneOwnerId",
                table: "Games");

            migrationBuilder.DropTable(
                name: "GameZoneOwners");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "GameZoneOwnerId",
                table: "Games",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_GameZoneOwnerId",
                table: "Games",
                newName: "IX_Games_UserId");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GameCategory",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameCategory", x => x.CategoryId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_CategoryId",
                table: "Games",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_GameCategory_CategoryId",
                table: "Games",
                column: "CategoryId",
                principalTable: "GameCategory",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Tblusers_UserId",
                table: "Games",
                column: "UserId",
                principalTable: "Tblusers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_GameCategory_CategoryId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Tblusers_UserId",
                table: "Games");

            migrationBuilder.DropTable(
                name: "GameCategory");

            migrationBuilder.DropIndex(
                name: "IX_Games_CategoryId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Games",
                newName: "GameZoneOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_UserId",
                table: "Games",
                newName: "IX_Games_GameZoneOwnerId");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Games",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GameZoneOwners",
                columns: table => new
                {
                    OwnerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameZoneOwners", x => x.OwnerId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Games_GameZoneOwners_GameZoneOwnerId",
                table: "Games",
                column: "GameZoneOwnerId",
                principalTable: "GameZoneOwners",
                principalColumn: "OwnerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketeer.Persistance.Database.Migrations
{
    public partial class Updatenewstomanytomanytickers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsArticles_Tickers_TickerId",
                table: "NewsArticles");

            migrationBuilder.DropIndex(
                name: "IX_NewsArticles_TickerId",
                table: "NewsArticles");

            migrationBuilder.DropColumn(
                name: "TickerId",
                table: "NewsArticles");

            migrationBuilder.CreateTable(
                name: "NewsArticleTicker",
                columns: table => new
                {
                    NewsArticlesId = table.Column<int>(type: "int", nullable: false),
                    TickersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsArticleTicker", x => new { x.NewsArticlesId, x.TickersId });
                    table.ForeignKey(
                        name: "FK_NewsArticleTicker_NewsArticles_NewsArticlesId",
                        column: x => x.NewsArticlesId,
                        principalTable: "NewsArticles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NewsArticleTicker_Tickers_TickersId",
                        column: x => x.TickersId,
                        principalTable: "Tickers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NewsArticleTicker_TickersId",
                table: "NewsArticleTicker",
                column: "TickersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewsArticleTicker");

            migrationBuilder.AddColumn<int>(
                name: "TickerId",
                table: "NewsArticles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NewsArticles_TickerId",
                table: "NewsArticles",
                column: "TickerId");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsArticles_Tickers_TickerId",
                table: "NewsArticles",
                column: "TickerId",
                principalTable: "Tickers",
                principalColumn: "Id");
        }
    }
}

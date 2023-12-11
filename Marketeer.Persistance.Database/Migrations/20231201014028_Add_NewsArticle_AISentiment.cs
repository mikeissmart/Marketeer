using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketeer.Persistance.Database.Migrations
{
    public partial class Add_NewsArticle_AISentiment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HuggingFaceModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HuggingFaceModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NewsArticles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TickerId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsArticles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewsArticles_Tickers_TickerId",
                        column: x => x.TickerId,
                        principalTable: "Tickers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SentimentResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HuggingFaceModelId = table.Column<int>(type: "int", nullable: false),
                    NewsArticleId = table.Column<int>(type: "int", nullable: true),
                    Negative = table.Column<float>(type: "real", nullable: false),
                    Neutral = table.Column<float>(type: "real", nullable: false),
                    Positive = table.Column<float>(type: "real", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SentimentResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SentimentResults_HuggingFaceModels_HuggingFaceModelId",
                        column: x => x.HuggingFaceModelId,
                        principalTable: "HuggingFaceModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SentimentResults_NewsArticles_NewsArticleId",
                        column: x => x.NewsArticleId,
                        principalTable: "NewsArticles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_HuggingFaceModels_Name",
                table: "HuggingFaceModels",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NewsArticles_TickerId",
                table: "NewsArticles",
                column: "TickerId");

            migrationBuilder.CreateIndex(
                name: "IX_SentimentResults_HuggingFaceModelId",
                table: "SentimentResults",
                column: "HuggingFaceModelId");

            migrationBuilder.CreateIndex(
                name: "IX_SentimentResults_NewsArticleId",
                table: "SentimentResults",
                column: "NewsArticleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SentimentResults");

            migrationBuilder.DropTable(
                name: "HuggingFaceModels");

            migrationBuilder.DropTable(
                name: "NewsArticles");
        }
    }
}

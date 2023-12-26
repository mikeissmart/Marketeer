using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketeer.Persistance.Database.Migrations
{
    public partial class UpdateSentimentResultsHuggingFaceModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SentimentResults_HuggingFaceModelId",
                table: "SentimentResults");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "SentimentResults",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "SentimentResults",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "HuggingFaceModels",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "HuggingFaceModels",
                columns: new[] { "Id", "IsDefault", "Name" },
                values: new object[] { 1, true, "mrm8488/distilroberta-finetuned-financial-news-sentiment-analysis" });

            migrationBuilder.CreateIndex(
                name: "IX_SentimentResults_HuggingFaceModelId_NewsArticleId",
                table: "SentimentResults",
                columns: new[] { "HuggingFaceModelId", "NewsArticleId" },
                unique: true,
                filter: "[NewsArticleId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SentimentResults_HuggingFaceModelId_NewsArticleId",
                table: "SentimentResults");

            migrationBuilder.DeleteData(
                table: "HuggingFaceModels",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "Status",
                table: "SentimentResults");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "SentimentResults");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "HuggingFaceModels");

            migrationBuilder.CreateIndex(
                name: "IX_SentimentResults_HuggingFaceModelId",
                table: "SentimentResults",
                column: "HuggingFaceModelId");
        }
    }
}

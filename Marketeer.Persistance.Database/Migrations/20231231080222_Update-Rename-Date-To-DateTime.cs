using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketeer.Persistance.Database.Migrations
{
    public partial class UpdateRenameDateToDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastNewsUpdate",
                table: "Tickers",
                newName: "LastNewsUpdateDateTime");

            migrationBuilder.RenameColumn(
                name: "LastInfoUpdate",
                table: "Tickers",
                newName: "LastInfoUpdateDateTime");

            migrationBuilder.RenameColumn(
                name: "LastHistoryUpdate",
                table: "Tickers",
                newName: "LastHistoryUpdateDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "TickerDelistReasons",
                newName: "CreatedDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "SentimentResults",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "SentimentResults",
                newName: "CreatedDateTime");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "PythonLogs",
                newName: "StartDateTime");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "PythonLogs",
                newName: "EndDateTime");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "NewsArticles",
                newName: "ArticleDate");

            migrationBuilder.RenameColumn(
                name: "MarketOpen",
                table: "MarketSchedules",
                newName: "MarketOpenDateTime");

            migrationBuilder.RenameColumn(
                name: "MarketClose",
                table: "MarketSchedules",
                newName: "MarketCloseDateTime");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "CronLogs",
                newName: "StartDateTime");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "CronLogs",
                newName: "EndDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "AppLogs",
                newName: "CreatedDateTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastNewsUpdateDateTime",
                table: "Tickers",
                newName: "LastNewsUpdate");

            migrationBuilder.RenameColumn(
                name: "LastInfoUpdateDateTime",
                table: "Tickers",
                newName: "LastInfoUpdate");

            migrationBuilder.RenameColumn(
                name: "LastHistoryUpdateDateTime",
                table: "Tickers",
                newName: "LastHistoryUpdate");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "TickerDelistReasons",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "SentimentResults",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "SentimentResults",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "StartDateTime",
                table: "PythonLogs",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "EndDateTime",
                table: "PythonLogs",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "ArticleDate",
                table: "NewsArticles",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "MarketOpenDateTime",
                table: "MarketSchedules",
                newName: "MarketOpen");

            migrationBuilder.RenameColumn(
                name: "MarketCloseDateTime",
                table: "MarketSchedules",
                newName: "MarketClose");

            migrationBuilder.RenameColumn(
                name: "StartDateTime",
                table: "CronLogs",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "EndDateTime",
                table: "CronLogs",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "AppLogs",
                newName: "CreatedDate");
        }
    }
}

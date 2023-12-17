using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketeer.Persistance.Database.Migrations
{
    public partial class Updaterenamedatetimestodate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Day",
                table: "MarketSchedules",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "HistoryDatas",
                newName: "Date");

            migrationBuilder.RenameIndex(
                name: "IX_HistoryDatas_TickerId_Interval_DateTime",
                table: "HistoryDatas",
                newName: "IX_HistoryDatas_TickerId_Interval_Date");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "MarketSchedules",
                newName: "Day");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "HistoryDatas",
                newName: "DateTime");

            migrationBuilder.RenameIndex(
                name: "IX_HistoryDatas_TickerId_Interval_Date",
                table: "HistoryDatas",
                newName: "IX_HistoryDatas_TickerId_Interval_DateTime");
        }
    }
}

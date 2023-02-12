using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketeer.Persistance.Database.Migrations
{
    public partial class Alter_TickerInfo_TickerSettings_PopularTickerInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelisted",
                table: "TickerInfos");

            migrationBuilder.AddColumn<bool>(
                name: "IsDelisted",
                table: "TickerSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UpdateDailyHistory",
                table: "TickerSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<long>(
                name: "Volume",
                table: "TickerInfos",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Sector",
                table: "TickerInfos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Industry",
                table: "TickerInfos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<float>(
                name: "DividendRate",
                table: "TickerInfos",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Exchange",
                table: "TickerInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "MarketCap",
                table: "TickerInfos",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "PayoutRatio",
                table: "TickerInfos",
                type: "real",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelisted",
                table: "TickerSettings");

            migrationBuilder.DropColumn(
                name: "UpdateDailyHistory",
                table: "TickerSettings");

            migrationBuilder.DropColumn(
                name: "DividendRate",
                table: "TickerInfos");

            migrationBuilder.DropColumn(
                name: "Exchange",
                table: "TickerInfos");

            migrationBuilder.DropColumn(
                name: "MarketCap",
                table: "TickerInfos");

            migrationBuilder.DropColumn(
                name: "PayoutRatio",
                table: "TickerInfos");

            migrationBuilder.AlterColumn<long>(
                name: "Volume",
                table: "TickerInfos",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Sector",
                table: "TickerInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Industry",
                table: "TickerInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelisted",
                table: "TickerInfos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

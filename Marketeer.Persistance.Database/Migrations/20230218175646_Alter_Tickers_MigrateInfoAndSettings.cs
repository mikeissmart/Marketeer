using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketeer.Persistance.Database.Migrations
{
    public partial class Alter_Tickers_MigrateInfoAndSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "DividendRate",
                table: "Tickers",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Exchange",
                table: "Tickers",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Industry",
                table: "Tickers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelisted",
                table: "Tickers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastHistoryUpdate",
                table: "Tickers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastInfoUpdate",
                table: "Tickers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "MarketCap",
                table: "Tickers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Tickers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "PayoutRatio",
                table: "Tickers",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuoteType",
                table: "Tickers",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sector",
                table: "Tickers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Volume",
                table: "Tickers",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DividendRate",
                table: "Tickers");

            migrationBuilder.DropColumn(
                name: "Exchange",
                table: "Tickers");

            migrationBuilder.DropColumn(
                name: "Industry",
                table: "Tickers");

            migrationBuilder.DropColumn(
                name: "IsDelisted",
                table: "Tickers");

            migrationBuilder.DropColumn(
                name: "LastHistoryUpdate",
                table: "Tickers");

            migrationBuilder.DropColumn(
                name: "LastInfoUpdate",
                table: "Tickers");

            migrationBuilder.DropColumn(
                name: "MarketCap",
                table: "Tickers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Tickers");

            migrationBuilder.DropColumn(
                name: "PayoutRatio",
                table: "Tickers");

            migrationBuilder.DropColumn(
                name: "QuoteType",
                table: "Tickers");

            migrationBuilder.DropColumn(
                name: "Sector",
                table: "Tickers");

            migrationBuilder.DropColumn(
                name: "Volume",
                table: "Tickers");
        }
    }
}

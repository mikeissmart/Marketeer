using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketeer.Persistance.Database.Migrations
{
    public partial class Alter_Ticker_Addmorecolumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Symbol",
                table: "Tickers",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(4)",
                oldMaxLength: 4);

            migrationBuilder.AddColumn<string>(
                name: "Industry",
                table: "Tickers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Tickers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QuoteType",
                table: "Tickers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sector",
                table: "Tickers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Volume",
                table: "Tickers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<decimal>(
                name: "Open",
                table: "HistoryDatas",
                type: "decimal(28,10)",
                precision: 28,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Low",
                table: "HistoryDatas",
                type: "decimal(28,10)",
                precision: 28,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "High",
                table: "HistoryDatas",
                type: "decimal(28,10)",
                precision: 28,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Close",
                table: "HistoryDatas",
                type: "decimal(28,10)",
                precision: 28,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,10)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Industry",
                table: "Tickers");

            migrationBuilder.DropColumn(
                name: "Name",
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

            migrationBuilder.AlterColumn<string>(
                name: "Symbol",
                table: "Tickers",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5);

            migrationBuilder.AlterColumn<decimal>(
                name: "Open",
                table: "HistoryDatas",
                type: "decimal(19,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(28,10)",
                oldPrecision: 28,
                oldScale: 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "Low",
                table: "HistoryDatas",
                type: "decimal(19,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(28,10)",
                oldPrecision: 28,
                oldScale: 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "High",
                table: "HistoryDatas",
                type: "decimal(19,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(28,10)",
                oldPrecision: 28,
                oldScale: 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "Close",
                table: "HistoryDatas",
                type: "decimal(19,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(28,10)",
                oldPrecision: 28,
                oldScale: 10);
        }
    }
}

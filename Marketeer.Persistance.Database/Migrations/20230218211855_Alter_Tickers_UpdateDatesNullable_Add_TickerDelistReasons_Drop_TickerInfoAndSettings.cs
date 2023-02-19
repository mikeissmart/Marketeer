using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketeer.Persistance.Database.Migrations
{
    public partial class Alter_Tickers_UpdateDatesNullable_Add_TickerDelistReasons_Drop_TickerInfoAndSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TickerInfos");

            migrationBuilder.DropTable(
                name: "TickerSettingHistoryDisables");

            migrationBuilder.DropTable(
                name: "TickerSettings");

            migrationBuilder.DropColumn(
                name: "IsDelisted",
                table: "Tickers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastInfoUpdate",
                table: "Tickers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastHistoryUpdate",
                table: "Tickers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "TickerDelistReasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TickerId = table.Column<int>(type: "int", nullable: false),
                    Delist = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TickerDelistReasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TickerDelistReasons_Tickers_TickerId",
                        column: x => x.TickerId,
                        principalTable: "Tickers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TickerDelistReasons_TickerId_Delist",
                table: "TickerDelistReasons",
                columns: new[] { "TickerId", "Delist" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TickerDelistReasons");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastInfoUpdate",
                table: "Tickers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastHistoryUpdate",
                table: "Tickers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelisted",
                table: "Tickers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "TickerInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TickerId = table.Column<int>(type: "int", nullable: false),
                    DividendRate = table.Column<float>(type: "real", nullable: true),
                    Exchange = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Industry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarketCap = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PayoutRatio = table.Column<float>(type: "real", nullable: true),
                    QuoteType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Volume = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TickerInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TickerInfos_Tickers_TickerId",
                        column: x => x.TickerId,
                        principalTable: "Tickers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TickerSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TickerId = table.Column<int>(type: "int", nullable: false),
                    IsDelisted = table.Column<bool>(type: "bit", nullable: false),
                    UpdateDailyHistory = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TickerSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TickerSettings_Tickers_TickerId",
                        column: x => x.TickerId,
                        principalTable: "Tickers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TickerSettingHistoryDisables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TickerSettingId = table.Column<int>(type: "int", nullable: false),
                    Interval = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TickerSettingHistoryDisables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TickerSettingHistoryDisables_TickerSettings_TickerSettingId",
                        column: x => x.TickerSettingId,
                        principalTable: "TickerSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TickerInfos_TickerId",
                table: "TickerInfos",
                column: "TickerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TickerSettingHistoryDisables_TickerSettingId",
                table: "TickerSettingHistoryDisables",
                column: "TickerSettingId");

            migrationBuilder.CreateIndex(
                name: "IX_TickerSettings_TickerId",
                table: "TickerSettings",
                column: "TickerId",
                unique: true);
        }
    }
}

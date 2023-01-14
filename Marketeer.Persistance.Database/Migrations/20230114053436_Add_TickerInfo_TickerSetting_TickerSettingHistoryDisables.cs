using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketeer.Persistance.Database.Migrations
{
    public partial class Add_TickerInfo_TickerSetting_TickerSettingHistoryDisables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TempDisabledFetchHistoryDatas");

            migrationBuilder.DropColumn(
                name: "Industry",
                table: "Tickers");

            migrationBuilder.DropColumn(
                name: "IsDelisted",
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

            migrationBuilder.CreateTable(
                name: "TickerInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TickerId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuoteType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sector = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Industry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Volume = table.Column<long>(type: "bigint", nullable: false),
                    IsDelisted = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                    TickerId = table.Column<int>(type: "int", nullable: false)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TickerInfos");

            migrationBuilder.DropTable(
                name: "TickerSettingHistoryDisables");

            migrationBuilder.DropTable(
                name: "TickerSettings");

            migrationBuilder.AddColumn<string>(
                name: "Industry",
                table: "Tickers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDelisted",
                table: "Tickers",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.CreateTable(
                name: "TempDisabledFetchHistoryDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TickerId = table.Column<int>(type: "int", nullable: false),
                    Interval = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempDisabledFetchHistoryDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TempDisabledFetchHistoryDatas_Tickers_TickerId",
                        column: x => x.TickerId,
                        principalTable: "Tickers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TempDisabledFetchHistoryDatas_TickerId",
                table: "TempDisabledFetchHistoryDatas",
                column: "TickerId",
                unique: true);
        }
    }
}

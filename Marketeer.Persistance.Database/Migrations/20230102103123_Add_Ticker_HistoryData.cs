using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketeer.Persistance.Database.Migrations
{
    public partial class Add_Ticker_HistoryData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ErrorOutput",
                table: "PythonLogs");

            migrationBuilder.AddColumn<string>(
                name: "Error",
                table: "PythonLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Output",
                table: "PythonLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tickers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    IsDelisted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HistoryDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TickerId = table.Column<int>(type: "int", nullable: false),
                    Interval = table.Column<int>(type: "int", nullable: false),
                    Open = table.Column<decimal>(type: "decimal(19,10)", nullable: false),
                    Close = table.Column<decimal>(type: "decimal(19,10)", nullable: false),
                    High = table.Column<decimal>(type: "decimal(19,10)", nullable: false),
                    Low = table.Column<decimal>(type: "decimal(19,10)", nullable: false),
                    Volume = table.Column<long>(type: "bigint", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoryDatas_Tickers_TickerId",
                        column: x => x.TickerId,
                        principalTable: "Tickers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistoryDatas_TickerId_Interval_DateTime",
                table: "HistoryDatas",
                columns: new[] { "TickerId", "Interval", "DateTime" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickers_Symbol",
                table: "Tickers",
                column: "Symbol",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoryDatas");

            migrationBuilder.DropTable(
                name: "Tickers");

            migrationBuilder.DropColumn(
                name: "Error",
                table: "PythonLogs");

            migrationBuilder.DropColumn(
                name: "Output",
                table: "PythonLogs");

            migrationBuilder.AddColumn<string>(
                name: "ErrorOutput",
                table: "PythonLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

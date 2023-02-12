using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketeer.Persistance.Database.Migrations
{
    public partial class Add_JsonTickerInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JsonTickerInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    InfoJson = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JsonTickerInfos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JsonTickerInfos");
        }
    }
}

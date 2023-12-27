using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketeer.Persistance.Database.Migrations
{
    public partial class UpdateCronLogCronLogTypeEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CronLogType",
                table: "CronLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CronLogType",
                table: "CronLogs");
        }
    }
}

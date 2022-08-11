using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnoopRatt.App.Migrations
{
    public partial class UpdateGuildSettingsTableAddTimezoneColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TimeZone",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeZone",
                table: "Settings");
        }
    }
}

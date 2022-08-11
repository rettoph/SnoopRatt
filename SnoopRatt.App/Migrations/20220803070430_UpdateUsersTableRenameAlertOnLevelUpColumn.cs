using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnoopRatt.App.Migrations
{
    public partial class UpdateUsersTableRenameAlertOnLevelUpColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AlertLevelUp",
                table: "Users",
                newName: "AlertOnLevelUp");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AlertOnLevelUp",
                table: "Users",
                newName: "AlertLevelUp");
        }
    }
}

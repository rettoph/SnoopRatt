using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnoopRatt.App.Migrations
{
    public partial class UpdateRoleMentionsTableRenameTypeColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "RoleMentions",
                newName: "Period");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Period",
                table: "RoleMentions",
                newName: "Type");
        }
    }
}

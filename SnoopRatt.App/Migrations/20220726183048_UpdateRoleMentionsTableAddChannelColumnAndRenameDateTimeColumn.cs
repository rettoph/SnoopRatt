using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnoopRatt.App.Migrations
{
    public partial class UpdateRoleMentionsTableAddChannelColumnAndRenameDateTimeColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "RoleMentions",
                newName: "TimeStamp");

            migrationBuilder.RenameIndex(
                name: "IX_RoleMentions_DateTime",
                table: "RoleMentions",
                newName: "IX_RoleMentions_TimeStamp");

            migrationBuilder.AddColumn<ulong>(
                name: "Channel",
                table: "RoleMentions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Channel",
                table: "RoleMentions");

            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "RoleMentions",
                newName: "DateTime");

            migrationBuilder.RenameIndex(
                name: "IX_RoleMentions_TimeStamp",
                table: "RoleMentions",
                newName: "IX_RoleMentions_DateTime");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnoopRatt.App.Migrations
{
    public partial class CreateUsersTableAndXpColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Settings",
                table: "Settings");

            migrationBuilder.RenameTable(
                name: "Settings",
                newName: "GuildSettings");

            migrationBuilder.AddColumn<double>(
                name: "Xp",
                table: "RoleMentions",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GuildSettings",
                table: "GuildSettings",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Xp = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GuildSettings",
                table: "GuildSettings");

            migrationBuilder.DropColumn(
                name: "Xp",
                table: "RoleMentions");

            migrationBuilder.RenameTable(
                name: "GuildSettings",
                newName: "Settings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Settings",
                table: "Settings",
                column: "Id");
        }
    }
}

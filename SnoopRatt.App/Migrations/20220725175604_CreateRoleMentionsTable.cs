using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnoopRatt.App.Migrations
{
    public partial class CreateRoleMentionsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoleMentions",
                columns: table => new
                {
                    Message = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Guild = table.Column<ulong>(type: "INTEGER", nullable: false),
                    User = table.Column<ulong>(type: "INTEGER", nullable: false),
                    DateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleMentions", x => x.Message);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleMentions_DateTime",
                table: "RoleMentions",
                column: "DateTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleMentions");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBSD_17037_16777_17286.Migrations
{
    public partial class fixSpellingMistake2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isMarried",
                table: "Employees",
                newName: "IsMarried");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsMarried",
                table: "Employees",
                newName: "isMarried");
        }
    }
}

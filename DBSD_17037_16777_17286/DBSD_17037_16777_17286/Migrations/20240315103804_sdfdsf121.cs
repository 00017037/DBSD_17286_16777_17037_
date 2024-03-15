using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBSD_17037_16777_17286.Migrations
{
    public partial class sdfdsf121 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employees_Id",
                table: "Employees");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Employees_Id",
                table: "Employees",
                column: "Id",
                filter: "[ManagerId] IS NOT NULL");
        }
    }
}

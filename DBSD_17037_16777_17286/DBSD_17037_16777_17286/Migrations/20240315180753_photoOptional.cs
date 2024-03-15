using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBSD_17037_16777_17286.Migrations
{
    public partial class photoOptional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "EmployeeViewModel",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Depatment",
                table: "EmployeeViewModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManagerName",
                table: "EmployeeViewModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManagerSurname",
                table: "EmployeeViewModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PersonId",
                table: "EmployeeViewModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<byte[]>(
                name: "Photo",
                table: "Employees",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Id",
                table: "Employees",
                column: "Id",
                filter: "[ManagerId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employees_Id",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Depatment",
                table: "EmployeeViewModel");

            migrationBuilder.DropColumn(
                name: "ManagerName",
                table: "EmployeeViewModel");

            migrationBuilder.DropColumn(
                name: "ManagerSurname",
                table: "EmployeeViewModel");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "EmployeeViewModel");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "EmployeeViewModel",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Photo",
                table: "Employees",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);
        }
    }
}

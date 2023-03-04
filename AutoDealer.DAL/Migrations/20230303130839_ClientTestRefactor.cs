using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoDealer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ClientTestRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_tests_employees",
                table: "tests");

            migrationBuilder.RenameColumn(
                name: "id_employee",
                table: "tests",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_tests_id_employee",
                table: "tests",
                newName: "IX_tests_EmployeeId");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "certification_date",
                table: "test_autos",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddForeignKey(
                name: "FK_tests_employees_EmployeeId",
                table: "tests",
                column: "EmployeeId",
                principalTable: "employees",
                principalColumn: "id_employee");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tests_employees_EmployeeId",
                table: "tests");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "tests",
                newName: "id_employee");

            migrationBuilder.RenameIndex(
                name: "IX_tests_EmployeeId",
                table: "tests",
                newName: "IX_tests_id_employee");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "certification_date",
                table: "test_autos",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_tests_employees",
                table: "tests",
                column: "id_employee",
                principalTable: "employees",
                principalColumn: "id_employee",
                onDelete: ReferentialAction.SetNull);
        }
    }
}

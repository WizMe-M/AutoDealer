using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoDealer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeleteUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_users_employees",
                table: "users");

            migrationBuilder.AddForeignKey(
                name: "fk_users_employees",
                table: "users",
                column: "id_employee",
                principalTable: "employees",
                principalColumn: "id_employee",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_users_employees",
                table: "users");

            migrationBuilder.AddForeignKey(
                name: "fk_users_employees",
                table: "users",
                column: "id_employee",
                principalTable: "employees",
                principalColumn: "id_employee");
        }
    }
}

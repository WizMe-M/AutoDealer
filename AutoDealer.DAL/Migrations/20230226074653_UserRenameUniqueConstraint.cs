using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoDealer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UserRenameUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "login",
                table: "users",
                newName: "email");

            migrationBuilder.RenameIndex(
                name: "uq_users_login",
                table: "users",
                newName: "uq_users_email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "email",
                table: "users",
                newName: "login");

            migrationBuilder.RenameIndex(
                name: "uq_users_email",
                table: "users",
                newName: "uq_users_login");
        }
    }
}

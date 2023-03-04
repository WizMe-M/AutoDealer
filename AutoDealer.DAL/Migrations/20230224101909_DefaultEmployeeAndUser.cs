using AutoDealer.DAL.Database.Entity;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoDealer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class DefaultEmployeeAndUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "employees",
                columns: new[] { "id_employee", "first_name", "last_name", "middle_name", "passport_number", "passport_series", "post" },
                values: new object[] { 1, "Maxim", "Timkin", null, "975717", "1199", Post.DatabaseAdmin });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id_employee", "deleted", "login", "password" },
                values: new object[] { 1, false, "timkin.moxim@mail.ru", "1ED6D5667B292B55FE629FCACB0027C808D6686C8C24B045E15212FC0207C73EBBC97F796695FCD306E2E4D3E8CCBF64C031221403023CEBFE86738119C97C20" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id_employee",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "employees",
                keyColumn: "id_employee",
                keyValue: 1);
        }
    }
}

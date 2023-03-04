using AutoDealer.DAL.Database.Entity;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AutoDealer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class PresetData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "car_models",
                columns: new[] { "id_car_model", "line_name", "model_name", "trim_code" },
                values: new object[] { 1, "Sun", "Crawler", "SC-4" });

            migrationBuilder.InsertData(
                table: "employees",
                columns: new[] { "id_employee", "first_name", "last_name", "middle_name", "passport_number", "passport_series", "post" },
                values: new object[,]
                {
                    { 2, "Ivan", "Ivanov", null, "111111", "1111", Post.AssemblyChief },
                    { 3, "Andrey", "Andreev", null, "222222", "2222", Post.PurchaseSpecialist },
                    { 4, "Igor", "Igorev", null, "333333", "3333", Post.Storekeeper },
                    { 5, "Sergey", "Sergeev", null, "444444", "4444", Post.Seller },
                    { 6, "Alexey", "Alexeev", null, "555555", "5555", Post.Tester }
                });

            migrationBuilder.InsertData(
                table: "suppliers",
                columns: new[] { "id_supplier", "correspondent_account", "legal_address", "postal_address", "settlement_account", "tin" },
                values: new object[] { 1, "30101810600000000957", "г. Москва, ул. Ленина, 19", "г. Ленинград, ул. Дзерджинского, 17б", "40817810099910004312", "123456789000" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id_employee",
                keyValue: 1,
                column: "email",
                value: "db@mail.ru");

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id_employee", "deleted", "email", "password" },
                values: new object[,]
                {
                    { 2, false, "chief@mail.ru", "1ED6D5667B292B55FE629FCACB0027C808D6686C8C24B045E15212FC0207C73EBBC97F796695FCD306E2E4D3E8CCBF64C031221403023CEBFE86738119C97C20" },
                    { 3, false, "spec@mail.ru", "1ED6D5667B292B55FE629FCACB0027C808D6686C8C24B045E15212FC0207C73EBBC97F796695FCD306E2E4D3E8CCBF64C031221403023CEBFE86738119C97C20" },
                    { 4, false, "store@mail.ru", "1ED6D5667B292B55FE629FCACB0027C808D6686C8C24B045E15212FC0207C73EBBC97F796695FCD306E2E4D3E8CCBF64C031221403023CEBFE86738119C97C20" },
                    { 5, false, "sell@mail.ru", "1ED6D5667B292B55FE629FCACB0027C808D6686C8C24B045E15212FC0207C73EBBC97F796695FCD306E2E4D3E8CCBF64C031221403023CEBFE86738119C97C20" },
                    { 6, false, "test@mail.ru", "1ED6D5667B292B55FE629FCACB0027C808D6686C8C24B045E15212FC0207C73EBBC97F796695FCD306E2E4D3E8CCBF64C031221403023CEBFE86738119C97C20" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "car_models",
                keyColumn: "id_car_model",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "suppliers",
                keyColumn: "id_supplier",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id_employee",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id_employee",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id_employee",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id_employee",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id_employee",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "employees",
                keyColumn: "id_employee",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "employees",
                keyColumn: "id_employee",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "employees",
                keyColumn: "id_employee",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "employees",
                keyColumn: "id_employee",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "employees",
                keyColumn: "id_employee",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id_employee",
                keyValue: 1,
                column: "email",
                value: "timkin.moxim@mail.ru");
        }
    }
}

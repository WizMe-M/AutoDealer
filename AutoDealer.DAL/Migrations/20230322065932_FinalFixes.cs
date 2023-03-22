using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoDealer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FinalFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_autos_trims",
                table: "autos");

            migrationBuilder.DropForeignKey(
                name: "fk_car_model_details_detail_series",
                table: "car_model_details");

            migrationBuilder.DropForeignKey(
                name: "fk_car_model_details_trims",
                table: "car_model_details");

            migrationBuilder.DropForeignKey(
                name: "fk_contract_details_contracts",
                table: "contract_details");

            migrationBuilder.DropForeignKey(
                name: "fk_contract_details_detail_series",
                table: "contract_details");

            migrationBuilder.DropForeignKey(
                name: "fk_margins_trims",
                table: "margins");

            migrationBuilder.DropForeignKey(
                name: "fk_test_autos_autos",
                table: "test_autos");

            migrationBuilder.DropForeignKey(
                name: "fk_test_autos_tests",
                table: "test_autos");

            migrationBuilder.DropForeignKey(
                name: "FK_tests_employees_EmployeeId",
                table: "tests");

            migrationBuilder.DropIndex(
                name: "IX_tests_EmployeeId",
                table: "tests");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "tests");

            migrationBuilder.AddForeignKey(
                name: "fk_autos_car_models",
                table: "autos",
                column: "id_car_model",
                principalTable: "car_models",
                principalColumn: "id_car_model",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_car_model_details_car_models",
                table: "car_model_details",
                column: "id_car_model",
                principalTable: "car_models",
                principalColumn: "id_car_model",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_car_model_details_detail_series",
                table: "car_model_details",
                column: "id_detail_series",
                principalTable: "detail_series",
                principalColumn: "id_detail_series",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_contract_details_contracts",
                table: "contract_details",
                column: "id_contract",
                principalTable: "contracts",
                principalColumn: "id_contract",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_contract_details_detail_series",
                table: "contract_details",
                column: "id_detail_series",
                principalTable: "detail_series",
                principalColumn: "id_detail_series",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_margins_car_models",
                table: "margins",
                column: "id_car_model",
                principalTable: "car_models",
                principalColumn: "id_car_model",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_test_autos_autos",
                table: "test_autos",
                column: "id_auto",
                principalTable: "autos",
                principalColumn: "id_auto",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_test_autos_tests",
                table: "test_autos",
                column: "id_test",
                principalTable: "tests",
                principalColumn: "id_test",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_autos_car_models",
                table: "autos");

            migrationBuilder.DropForeignKey(
                name: "fk_car_model_details_car_models",
                table: "car_model_details");

            migrationBuilder.DropForeignKey(
                name: "fk_car_model_details_detail_series",
                table: "car_model_details");

            migrationBuilder.DropForeignKey(
                name: "fk_contract_details_contracts",
                table: "contract_details");

            migrationBuilder.DropForeignKey(
                name: "fk_contract_details_detail_series",
                table: "contract_details");

            migrationBuilder.DropForeignKey(
                name: "fk_margins_car_models",
                table: "margins");

            migrationBuilder.DropForeignKey(
                name: "fk_test_autos_autos",
                table: "test_autos");

            migrationBuilder.DropForeignKey(
                name: "fk_test_autos_tests",
                table: "test_autos");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "tests",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tests_EmployeeId",
                table: "tests",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "fk_autos_trims",
                table: "autos",
                column: "id_car_model",
                principalTable: "car_models",
                principalColumn: "id_car_model");

            migrationBuilder.AddForeignKey(
                name: "fk_car_model_details_detail_series",
                table: "car_model_details",
                column: "id_detail_series",
                principalTable: "detail_series",
                principalColumn: "id_detail_series");

            migrationBuilder.AddForeignKey(
                name: "fk_car_model_details_trims",
                table: "car_model_details",
                column: "id_car_model",
                principalTable: "car_models",
                principalColumn: "id_car_model");

            migrationBuilder.AddForeignKey(
                name: "fk_contract_details_contracts",
                table: "contract_details",
                column: "id_contract",
                principalTable: "contracts",
                principalColumn: "id_contract");

            migrationBuilder.AddForeignKey(
                name: "fk_contract_details_detail_series",
                table: "contract_details",
                column: "id_detail_series",
                principalTable: "detail_series",
                principalColumn: "id_detail_series");

            migrationBuilder.AddForeignKey(
                name: "fk_margins_trims",
                table: "margins",
                column: "id_car_model",
                principalTable: "car_models",
                principalColumn: "id_car_model");

            migrationBuilder.AddForeignKey(
                name: "fk_test_autos_autos",
                table: "test_autos",
                column: "id_auto",
                principalTable: "autos",
                principalColumn: "id_auto");

            migrationBuilder.AddForeignKey(
                name: "fk_test_autos_tests",
                table: "test_autos",
                column: "id_test",
                principalTable: "tests",
                principalColumn: "id_test");

            migrationBuilder.AddForeignKey(
                name: "FK_tests_employees_EmployeeId",
                table: "tests",
                column: "EmployeeId",
                principalTable: "employees",
                principalColumn: "id_employee");
        }
    }
}

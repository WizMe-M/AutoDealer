using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AutoDealer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemovePurchaseRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_contracts_purchase_requests_PurchaseRequestId",
                table: "contracts");

            migrationBuilder.DropTable(
                name: "purchase_request_details");

            migrationBuilder.DropTable(
                name: "purchase_requests");

            migrationBuilder.DropIndex(
                name: "IX_contracts_PurchaseRequestId",
                table: "contracts");

            migrationBuilder.DropColumn(
                name: "PurchaseRequestId",
                table: "contracts");

            migrationBuilder.RenameColumn(
                name: "id_employee",
                table: "contracts",
                newName: "id_storekeeper");

            migrationBuilder.RenameIndex(
                name: "IX_contracts_id_employee",
                table: "contracts",
                newName: "IX_contracts_id_storekeeper");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:auto_status", "assembled,testing,selling,sold")
                .Annotation("Npgsql:Enum:log_type", "error,normal")
                .Annotation("Npgsql:Enum:post", "database_admin,assembly_chief,purchase_specialist,storekeeper,seller,tester")
                .Annotation("Npgsql:Enum:test_status", "not_checked,certified,defective")
                .OldAnnotation("Npgsql:Enum:auto_status", "assembled,testing,selling,sold")
                .OldAnnotation("Npgsql:Enum:log_type", "error,normal")
                .OldAnnotation("Npgsql:Enum:post", "database_admin,assembly_chief,purchase_specialist,storekeeper,seller,tester")
                .OldAnnotation("Npgsql:Enum:request_status", "sent,in_handling,closed")
                .OldAnnotation("Npgsql:Enum:test_status", "not_checked,certified,defective");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id_storekeeper",
                table: "contracts",
                newName: "id_employee");

            migrationBuilder.RenameIndex(
                name: "IX_contracts_id_storekeeper",
                table: "contracts",
                newName: "IX_contracts_id_employee");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:auto_status", "assembled,testing,selling,sold")
                .Annotation("Npgsql:Enum:log_type", "error,normal")
                .Annotation("Npgsql:Enum:post", "database_admin,assembly_chief,purchase_specialist,storekeeper,seller,tester")
                .Annotation("Npgsql:Enum:request_status", "sent,in_handling,closed")
                .Annotation("Npgsql:Enum:test_status", "not_checked,certified,defective")
                .OldAnnotation("Npgsql:Enum:auto_status", "assembled,testing,selling,sold")
                .OldAnnotation("Npgsql:Enum:log_type", "error,normal")
                .OldAnnotation("Npgsql:Enum:post", "database_admin,assembly_chief,purchase_specialist,storekeeper,seller,tester")
                .OldAnnotation("Npgsql:Enum:test_status", "not_checked,certified,defective");

            migrationBuilder.AddColumn<int>(
                name: "PurchaseRequestId",
                table: "contracts",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "purchase_requests",
                columns: table => new
                {
                    id_purchase_requests = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_user = table.Column<int>(type: "integer", nullable: true),
                    expected_supply_date = table.Column<DateOnly>(type: "date", nullable: false),
                    sent_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    status = table.Column<int>(type: "request_status", nullable: false, defaultValueSql: "'sent'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_purchase_requests", x => x.id_purchase_requests);
                    table.ForeignKey(
                        name: "fk_purchase_requests_users",
                        column: x => x.id_user,
                        principalTable: "users",
                        principalColumn: "id_employee",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "purchase_request_details",
                columns: table => new
                {
                    id_purchase_request = table.Column<int>(type: "integer", nullable: false),
                    id_detail_series = table.Column<int>(type: "integer", nullable: false),
                    count = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_purchase_request_details", x => new { x.id_purchase_request, x.id_detail_series });
                    table.ForeignKey(
                        name: "fk_purchase_request_details_detail_series",
                        column: x => x.id_detail_series,
                        principalTable: "detail_series",
                        principalColumn: "id_detail_series");
                    table.ForeignKey(
                        name: "fk_purchase_request_details_purchase_requests",
                        column: x => x.id_purchase_request,
                        principalTable: "purchase_requests",
                        principalColumn: "id_purchase_requests");
                });

            migrationBuilder.CreateIndex(
                name: "IX_contracts_PurchaseRequestId",
                table: "contracts",
                column: "PurchaseRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_request_details_id_detail_series",
                table: "purchase_request_details",
                column: "id_detail_series");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_requests_id_user",
                table: "purchase_requests",
                column: "id_user");

            migrationBuilder.AddForeignKey(
                name: "FK_contracts_purchase_requests_PurchaseRequestId",
                table: "contracts",
                column: "PurchaseRequestId",
                principalTable: "purchase_requests",
                principalColumn: "id_purchase_requests");
        }
    }
}

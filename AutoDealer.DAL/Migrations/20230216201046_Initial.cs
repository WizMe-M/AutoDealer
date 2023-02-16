using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AutoDealer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:auto_status", "in_assembly,ready_to_test,in_test,ready_to_sale,sold")
                .Annotation("Npgsql:Enum:log_type", "error,normal")
                .Annotation("Npgsql:Enum:post", "database_admin,assembly_chief,purchase_specialist,storekeeper,seller,tester")
                .Annotation("Npgsql:Enum:request_status", "sent,in_handling,closed")
                .Annotation("Npgsql:Enum:test_status", "not_checked,certified,defective");

            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    id_client = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    middle_name = table.Column<string>(type: "text", nullable: true),
                    birthdate = table.Column<DateOnly>(type: "date", nullable: false),
                    birthplace = table.Column<string>(type: "text", nullable: false),
                    passport_series = table.Column<string>(type: "text", nullable: false),
                    passport_number = table.Column<string>(type: "text", nullable: false),
                    passport_issuer = table.Column<string>(type: "text", nullable: false),
                    department_code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clients", x => x.id_client);
                });

            migrationBuilder.CreateTable(
                name: "detail_series",
                columns: table => new
                {
                    id_detail_series = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_detail_series", x => x.id_detail_series);
                });

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    id_employee = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    middle_name = table.Column<string>(type: "text", nullable: true),
                    passport_series = table.Column<string>(type: "text", nullable: false),
                    passport_number = table.Column<string>(type: "text", nullable: false),
                    post = table.Column<string>(type: "Post", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employee", x => x.id_employee);
                });

            migrationBuilder.CreateTable(
                name: "lines",
                columns: table => new
                {
                    id_line = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lines", x => x.id_line);
                });

            migrationBuilder.CreateTable(
                name: "logs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    log_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    log_text = table.Column<string>(type: "text", nullable: false),
                    log_type = table.Column<string>(type: "LogType", nullable: false, defaultValueSql: "Normal")
                },
                constraints: table =>
                {
                    table.PrimaryKey("logs_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "suppliers",
                columns: table => new
                {
                    id_supplier = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    legal_address = table.Column<string>(type: "text", nullable: false),
                    postal_address = table.Column<string>(type: "text", nullable: false),
                    correspondent_account = table.Column<string>(type: "text", nullable: false),
                    settlement_account = table.Column<string>(type: "text", nullable: false),
                    tin = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_suppliers", x => x.id_supplier);
                });

            migrationBuilder.CreateTable(
                name: "work_plans",
                columns: table => new
                {
                    id_work_plan = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    conclusion_date = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "now()"),
                    work_start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    work_end_date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_work_plans", x => x.id_work_plan);
                });

            migrationBuilder.CreateTable(
                name: "tests",
                columns: table => new
                {
                    id_test = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_employee = table.Column<int>(type: "integer", nullable: true),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tests", x => x.id_test);
                    table.ForeignKey(
                        name: "fk_tests_employees",
                        column: x => x.id_employee,
                        principalTable: "employees",
                        principalColumn: "id_employee",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id_employee = table.Column<int>(type: "integer", nullable: false),
                    login = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users_employees", x => x.id_employee);
                    table.ForeignKey(
                        name: "fk_users_employees",
                        column: x => x.id_employee,
                        principalTable: "employees",
                        principalColumn: "id_employee");
                });

            migrationBuilder.CreateTable(
                name: "models",
                columns: table => new
                {
                    id_model = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_line = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_models", x => x.id_model);
                    table.ForeignKey(
                        name: "fk_models_lines",
                        column: x => x.id_line,
                        principalTable: "lines",
                        principalColumn: "id_line");
                });

            migrationBuilder.CreateTable(
                name: "purchase_requests",
                columns: table => new
                {
                    id_purchase_requests = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_user = table.Column<int>(type: "integer", nullable: true),
                    sent_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    expected_supply_date = table.Column<DateOnly>(type: "date", nullable: false),
                    status = table.Column<string>(type: "RequestStatus", nullable: false, defaultValueSql: "Sent")
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
                name: "trims",
                columns: table => new
                {
                    id_trim = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_model = table.Column<int>(type: "integer", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_trims", x => x.id_trim);
                    table.ForeignKey(
                        name: "fk_trims_models",
                        column: x => x.id_model,
                        principalTable: "models",
                        principalColumn: "id_model");
                });

            migrationBuilder.CreateTable(
                name: "contracts",
                columns: table => new
                {
                    id_contract = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_employee = table.Column<int>(type: "integer", nullable: true),
                    id_supplier = table.Column<int>(type: "integer", nullable: true),
                    id_purchase_request = table.Column<int>(type: "integer", nullable: true),
                    conclusion_date = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "now()"),
                    supply_date = table.Column<DateOnly>(type: "date", nullable: false),
                    total_sum = table.Column<decimal>(type: "numeric", nullable: false),
                    lading_bill_issue_date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contracts", x => x.id_contract);
                    table.ForeignKey(
                        name: "fk_contracts_employees",
                        column: x => x.id_employee,
                        principalTable: "employees",
                        principalColumn: "id_employee");
                    table.ForeignKey(
                        name: "fk_contracts_purchase_requests",
                        column: x => x.id_purchase_request,
                        principalTable: "purchase_requests",
                        principalColumn: "id_purchase_requests");
                    table.ForeignKey(
                        name: "fk_contracts_suppliers",
                        column: x => x.id_supplier,
                        principalTable: "suppliers",
                        principalColumn: "id_supplier");
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

            migrationBuilder.CreateTable(
                name: "autos",
                columns: table => new
                {
                    id_auto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_trim = table.Column<int>(type: "integer", nullable: false),
                    assembly_date = table.Column<DateOnly>(type: "date", nullable: true),
                    cost = table.Column<decimal>(type: "numeric", nullable: true),
                    status = table.Column<string>(type: "AutoStatus", nullable: false, defaultValueSql: "InAssembly")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_autos", x => x.id_auto);
                    table.ForeignKey(
                        name: "fk_autos_trims",
                        column: x => x.id_trim,
                        principalTable: "trims",
                        principalColumn: "id_trim");
                });

            migrationBuilder.CreateTable(
                name: "margins",
                columns: table => new
                {
                    id_trim = table.Column<int>(type: "integer", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    margin = table.Column<decimal>(type: "numeric", nullable: false, defaultValueSql: "10")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_margins", x => new { x.id_trim, x.start_date });
                    table.ForeignKey(
                        name: "fk_margins_trims",
                        column: x => x.id_trim,
                        principalTable: "trims",
                        principalColumn: "id_trim");
                });

            migrationBuilder.CreateTable(
                name: "trim_details",
                columns: table => new
                {
                    id_trim = table.Column<int>(type: "integer", nullable: false),
                    id_detail_series = table.Column<int>(type: "integer", nullable: false),
                    count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_trim_details", x => new { x.id_trim, x.id_detail_series });
                    table.ForeignKey(
                        name: "fk_trim_details_detail_series",
                        column: x => x.id_detail_series,
                        principalTable: "detail_series",
                        principalColumn: "id_detail_series");
                    table.ForeignKey(
                        name: "fk_trim_details_trims",
                        column: x => x.id_trim,
                        principalTable: "trims",
                        principalColumn: "id_trim");
                });

            migrationBuilder.CreateTable(
                name: "contract_details",
                columns: table => new
                {
                    id_contract = table.Column<int>(type: "integer", nullable: false),
                    id_detail_series = table.Column<int>(type: "integer", nullable: false),
                    count = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "1"),
                    cost_per_one = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contract_details", x => new { x.id_contract, x.id_detail_series });
                    table.ForeignKey(
                        name: "fk_contract_details_contracts",
                        column: x => x.id_contract,
                        principalTable: "contracts",
                        principalColumn: "id_contract");
                    table.ForeignKey(
                        name: "fk_contract_details_detail_series",
                        column: x => x.id_detail_series,
                        principalTable: "detail_series",
                        principalColumn: "id_detail_series");
                });

            migrationBuilder.CreateTable(
                name: "details",
                columns: table => new
                {
                    id_detail_series = table.Column<int>(type: "integer", nullable: false),
                    id_detail = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_contract = table.Column<int>(type: "integer", nullable: false),
                    id_auto = table.Column<int>(type: "integer", nullable: true),
                    cost = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_details", x => new { x.id_detail_series, x.id_detail });
                    table.ForeignKey(
                        name: "fk_details_autos",
                        column: x => x.id_auto,
                        principalTable: "autos",
                        principalColumn: "id_auto",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_details_contracts",
                        column: x => x.id_contract,
                        principalTable: "contracts",
                        principalColumn: "id_contract",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_details_detail_series",
                        column: x => x.id_detail_series,
                        principalTable: "detail_series",
                        principalColumn: "id_detail_series",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sales",
                columns: table => new
                {
                    id_auto = table.Column<int>(type: "integer", nullable: false),
                    execution_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    id_client = table.Column<int>(type: "integer", nullable: false),
                    id_employee = table.Column<int>(type: "integer", nullable: false),
                    total_sum = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sales", x => new { x.id_auto, x.execution_date });
                    table.ForeignKey(
                        name: "fk_sales_autos",
                        column: x => x.id_auto,
                        principalTable: "autos",
                        principalColumn: "id_auto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_sales_clients",
                        column: x => x.id_client,
                        principalTable: "clients",
                        principalColumn: "id_client",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_sales_employees",
                        column: x => x.id_employee,
                        principalTable: "employees",
                        principalColumn: "id_employee",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "test_autos",
                columns: table => new
                {
                    id_test = table.Column<int>(type: "integer", nullable: false),
                    id_auto = table.Column<int>(type: "integer", nullable: false),
                    certification_date = table.Column<DateOnly>(type: "date", nullable: false),
                    status = table.Column<string>(type: "TestStatus", nullable: false, defaultValueSql: "NotChecked")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_test_autos", x => new { x.id_test, x.id_auto });
                    table.ForeignKey(
                        name: "fk_test_autos_autos",
                        column: x => x.id_auto,
                        principalTable: "autos",
                        principalColumn: "id_auto");
                    table.ForeignKey(
                        name: "fk_test_autos_tests",
                        column: x => x.id_test,
                        principalTable: "tests",
                        principalColumn: "id_test");
                });

            migrationBuilder.CreateTable(
                name: "works",
                columns: table => new
                {
                    id_work_plan = table.Column<int>(type: "integer", nullable: false),
                    id_auto = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_works", x => new { x.id_work_plan, x.id_auto });
                    table.ForeignKey(
                        name: "fk_works_autos",
                        column: x => x.id_auto,
                        principalTable: "autos",
                        principalColumn: "id_auto",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_works_work_plans",
                        column: x => x.id_work_plan,
                        principalTable: "work_plans",
                        principalColumn: "id_work_plan");
                });

            migrationBuilder.CreateIndex(
                name: "IX_autos_id_trim",
                table: "autos",
                column: "id_trim");

            migrationBuilder.CreateIndex(
                name: "IX_contract_details_id_detail_series",
                table: "contract_details",
                column: "id_detail_series");

            migrationBuilder.CreateIndex(
                name: "IX_contracts_id_employee",
                table: "contracts",
                column: "id_employee");

            migrationBuilder.CreateIndex(
                name: "IX_contracts_id_purchase_request",
                table: "contracts",
                column: "id_purchase_request");

            migrationBuilder.CreateIndex(
                name: "IX_contracts_id_supplier",
                table: "contracts",
                column: "id_supplier");

            migrationBuilder.CreateIndex(
                name: "uq_detail_series_code",
                table: "detail_series",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_details_id_auto",
                table: "details",
                column: "id_auto");

            migrationBuilder.CreateIndex(
                name: "IX_details_id_contract",
                table: "details",
                column: "id_contract");

            migrationBuilder.CreateIndex(
                name: "uq_lines_name",
                table: "lines",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_models_id_line",
                table: "models",
                column: "id_line");

            migrationBuilder.CreateIndex(
                name: "uq_models_name",
                table: "models",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_purchase_request_details_id_detail_series",
                table: "purchase_request_details",
                column: "id_detail_series");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_requests_id_user",
                table: "purchase_requests",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_sales_id_client",
                table: "sales",
                column: "id_client");

            migrationBuilder.CreateIndex(
                name: "IX_sales_id_employee",
                table: "sales",
                column: "id_employee");

            migrationBuilder.CreateIndex(
                name: "IX_test_autos_id_auto",
                table: "test_autos",
                column: "id_auto");

            migrationBuilder.CreateIndex(
                name: "IX_tests_id_employee",
                table: "tests",
                column: "id_employee");

            migrationBuilder.CreateIndex(
                name: "IX_trim_details_id_detail_series",
                table: "trim_details",
                column: "id_detail_series");

            migrationBuilder.CreateIndex(
                name: "IX_trims_id_model",
                table: "trims",
                column: "id_model");

            migrationBuilder.CreateIndex(
                name: "uq_trims_code",
                table: "trims",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_users_login",
                table: "users",
                column: "login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_works_id_auto",
                table: "works",
                column: "id_auto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "contract_details");

            migrationBuilder.DropTable(
                name: "details");

            migrationBuilder.DropTable(
                name: "logs");

            migrationBuilder.DropTable(
                name: "margins");

            migrationBuilder.DropTable(
                name: "purchase_request_details");

            migrationBuilder.DropTable(
                name: "sales");

            migrationBuilder.DropTable(
                name: "test_autos");

            migrationBuilder.DropTable(
                name: "trim_details");

            migrationBuilder.DropTable(
                name: "works");

            migrationBuilder.DropTable(
                name: "contracts");

            migrationBuilder.DropTable(
                name: "clients");

            migrationBuilder.DropTable(
                name: "tests");

            migrationBuilder.DropTable(
                name: "detail_series");

            migrationBuilder.DropTable(
                name: "autos");

            migrationBuilder.DropTable(
                name: "work_plans");

            migrationBuilder.DropTable(
                name: "purchase_requests");

            migrationBuilder.DropTable(
                name: "suppliers");

            migrationBuilder.DropTable(
                name: "trims");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "models");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "lines");
        }
    }
}

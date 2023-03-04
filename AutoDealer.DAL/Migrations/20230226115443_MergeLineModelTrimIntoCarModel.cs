using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AutoDealer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class MergeLineModelTrimIntoCarModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_autos_trims",
                table: "autos");

            migrationBuilder.DropForeignKey(
                name: "fk_margins_trims",
                table: "margins");

            migrationBuilder.DropTable(
                name: "trim_details");

            migrationBuilder.DropTable(
                name: "trims");

            migrationBuilder.DropTable(
                name: "models");

            migrationBuilder.DropTable(
                name: "lines");

            migrationBuilder.CreateTable(
                name: "car_models",
                columns: table => new
                {
                    id_car_model = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    line_name = table.Column<string>(type: "text", nullable: false),
                    model_name = table.Column<string>(type: "text", nullable: false),
                    trim_code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_car_models", x => x.id_car_model);
                });

            migrationBuilder.CreateTable(
                name: "car_model_details",
                columns: table => new
                {
                    id_car_model = table.Column<int>(type: "integer", nullable: false),
                    id_detail_series = table.Column<int>(type: "integer", nullable: false),
                    count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_car_model_details", x => new { x.id_car_model, x.id_detail_series });
                    table.ForeignKey(
                        name: "fk_car_model_details_detail_series",
                        column: x => x.id_detail_series,
                        principalTable: "detail_series",
                        principalColumn: "id_detail_series");
                    table.ForeignKey(
                        name: "fk_car_model_details_trims",
                        column: x => x.id_car_model,
                        principalTable: "car_models",
                        principalColumn: "id_car_model");
                });

            migrationBuilder.CreateIndex(
                name: "IX_car_model_details_id_detail_series",
                table: "car_model_details",
                column: "id_detail_series");

            migrationBuilder.CreateIndex(
                name: "uq_car_models_name",
                table: "car_models",
                columns: new[] { "line_name", "model_name", "trim_code" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_autos_trims",
                table: "autos",
                column: "id_trim",
                principalTable: "car_models",
                principalColumn: "id_car_model");

            migrationBuilder.AddForeignKey(
                name: "fk_margins_trims",
                table: "margins",
                column: "id_trim",
                principalTable: "car_models",
                principalColumn: "id_car_model");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_autos_trims",
                table: "autos");

            migrationBuilder.DropForeignKey(
                name: "fk_margins_trims",
                table: "margins");

            migrationBuilder.DropTable(
                name: "car_model_details");

            migrationBuilder.DropTable(
                name: "car_models");

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

            migrationBuilder.AddForeignKey(
                name: "fk_autos_trims",
                table: "autos",
                column: "id_trim",
                principalTable: "trims",
                principalColumn: "id_trim");

            migrationBuilder.AddForeignKey(
                name: "fk_margins_trims",
                table: "margins",
                column: "id_trim",
                principalTable: "trims",
                principalColumn: "id_trim");
        }
    }
}

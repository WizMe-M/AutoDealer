using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AutoDealer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveWorkEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "works");

            migrationBuilder.DropTable(
                name: "work_plans");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "work_plans",
                columns: table => new
                {
                    id_work_plan = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    conclusion_date = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "now()"),
                    work_end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    work_start_date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_work_plans", x => x.id_work_plan);
                });

            migrationBuilder.CreateTable(
                name: "works",
                columns: table => new
                {
                    id_work_plan = table.Column<int>(type: "integer", nullable: false),
                    id_auto = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: false)
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
                name: "IX_works_id_auto",
                table: "works",
                column: "id_auto");
        }
    }
}

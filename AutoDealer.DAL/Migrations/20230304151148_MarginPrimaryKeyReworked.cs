using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AutoDealer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class MarginPrimaryKeyReworked : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_margins",
                table: "margins");

            migrationBuilder.RenameColumn(
                name: "id_trim",
                table: "margins",
                newName: "id_car_model");

            migrationBuilder.AddColumn<int>(
                name: "id_margin",
                table: "margins",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "pk_margins",
                table: "margins",
                column: "id_margin");

            migrationBuilder.CreateIndex(
                name: "IX_margins_id_car_model",
                table: "margins",
                column: "id_car_model");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_margins",
                table: "margins");

            migrationBuilder.DropIndex(
                name: "IX_margins_id_car_model",
                table: "margins");

            migrationBuilder.DropColumn(
                name: "id_margin",
                table: "margins");

            migrationBuilder.RenameColumn(
                name: "id_car_model",
                table: "margins",
                newName: "id_trim");

            migrationBuilder.AddPrimaryKey(
                name: "pk_margins",
                table: "margins",
                columns: new[] { "id_trim", "start_date" });
        }
    }
}

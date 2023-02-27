using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoDealer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class DetailSeriesPresetData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "detail_series",
                columns: new[] { "id_detail_series", "code", "description" },
                values: new object[] { 1, "SDH-242-790.1", "Full completed and assembled automobile" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "detail_series",
                keyColumn: "id_detail_series",
                keyValue: 1);
        }
    }
}

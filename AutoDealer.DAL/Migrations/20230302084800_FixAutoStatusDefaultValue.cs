using AutoDealer.DAL.Database.Entity;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoDealer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixAutoStatusDefaultValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<AutoStatus>(
                name: "status",
                table: "autos",
                type: "auto_status",
                nullable: false,
                defaultValueSql: "'assembled'",
                oldClrType: typeof(AutoStatus),
                oldType: "auto_status",
                oldDefaultValueSql: "'in_assembly'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<AutoStatus>(
                name: "status",
                table: "autos",
                type: "auto_status",
                nullable: false,
                defaultValueSql: "'in_assembly'",
                oldClrType: typeof(AutoStatus),
                oldType: "auto_status",
                oldDefaultValueSql: "'assembled'");
        }
    }
}

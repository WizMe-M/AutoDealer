using AutoDealer.DAL.Database.Entity;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AutoDealer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class LogOptimization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "logs_pkey",
                table: "logs");

            migrationBuilder.DropColumn(
                name: "id",
                table: "logs");

            migrationBuilder.DropColumn(
                name: "log_type",
                table: "logs");

            migrationBuilder.AddPrimaryKey(
                name: "logs_pkey",
                table: "logs",
                column: "log_time");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "logs_pkey",
                table: "logs");

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "logs",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<LogType>(
                name: "log_type",
                table: "logs",
                type: "log_type",
                nullable: false,
                defaultValueSql: "'normal'");

            migrationBuilder.AddPrimaryKey(
                name: "logs_pkey",
                table: "logs",
                column: "id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoDealer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class LittleRework : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_contracts_purchase_requests",
                table: "contracts");

            migrationBuilder.DropPrimaryKey(
                name: "pk_details",
                table: "details");

            migrationBuilder.RenameColumn(
                name: "id_purchase_request",
                table: "contracts",
                newName: "PurchaseRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_contracts_id_purchase_request",
                table: "contracts",
                newName: "IX_contracts_PurchaseRequestId");

            migrationBuilder.AddPrimaryKey(
                name: "pk_details",
                table: "details",
                column: "id_detail");

            migrationBuilder.CreateIndex(
                name: "IX_details_id_detail_series",
                table: "details",
                column: "id_detail_series");

            migrationBuilder.AddForeignKey(
                name: "FK_contracts_purchase_requests_PurchaseRequestId",
                table: "contracts",
                column: "PurchaseRequestId",
                principalTable: "purchase_requests",
                principalColumn: "id_purchase_requests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_contracts_purchase_requests_PurchaseRequestId",
                table: "contracts");

            migrationBuilder.DropPrimaryKey(
                name: "pk_details",
                table: "details");

            migrationBuilder.DropIndex(
                name: "IX_details_id_detail_series",
                table: "details");

            migrationBuilder.RenameColumn(
                name: "PurchaseRequestId",
                table: "contracts",
                newName: "id_purchase_request");

            migrationBuilder.RenameIndex(
                name: "IX_contracts_PurchaseRequestId",
                table: "contracts",
                newName: "IX_contracts_id_purchase_request");

            migrationBuilder.AddPrimaryKey(
                name: "pk_details",
                table: "details",
                columns: new[] { "id_detail_series", "id_detail" });

            migrationBuilder.AddForeignKey(
                name: "fk_contracts_purchase_requests",
                table: "contracts",
                column: "id_purchase_request",
                principalTable: "purchase_requests",
                principalColumn: "id_purchase_requests");
        }
    }
}

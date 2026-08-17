using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BhuttaBoutiqueERP.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceCatalogRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServiceCatalogId",
                table: "CommissionRates",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommissionRates_ServiceCatalogId",
                table: "CommissionRates",
                column: "ServiceCatalogId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommissionRates_ServiceCatalogs_ServiceCatalogId",
                table: "CommissionRates",
                column: "ServiceCatalogId",
                principalTable: "ServiceCatalogs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommissionRates_ServiceCatalogs_ServiceCatalogId",
                table: "CommissionRates");

            migrationBuilder.DropIndex(
                name: "IX_CommissionRates_ServiceCatalogId",
                table: "CommissionRates");

            migrationBuilder.DropColumn(
                name: "ServiceCatalogId",
                table: "CommissionRates");
        }
    }
}

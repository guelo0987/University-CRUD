using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Migrations
{
    /// <inheritdoc />
    public partial class AddCuentaPorPagar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facturas_CuentaPorPagars_CuentaPorPagarIdCuentaPorPagar",
                table: "Facturas");

            migrationBuilder.RenameColumn(
                name: "CuentaPorPagarIdCuentaPorPagar",
                table: "Facturas",
                newName: "CuentaPorPagarId");

            migrationBuilder.RenameIndex(
                name: "IX_Facturas_CuentaPorPagarIdCuentaPorPagar",
                table: "Facturas",
                newName: "IX_Facturas_CuentaPorPagarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Facturas_CuentaPorPagars_CuentaPorPagarId",
                table: "Facturas",
                column: "CuentaPorPagarId",
                principalTable: "CuentaPorPagars",
                principalColumn: "IdCuentaPorPagar");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facturas_CuentaPorPagars_CuentaPorPagarId",
                table: "Facturas");

            migrationBuilder.RenameColumn(
                name: "CuentaPorPagarId",
                table: "Facturas",
                newName: "CuentaPorPagarIdCuentaPorPagar");

            migrationBuilder.RenameIndex(
                name: "IX_Facturas_CuentaPorPagarId",
                table: "Facturas",
                newName: "IX_Facturas_CuentaPorPagarIdCuentaPorPagar");

            migrationBuilder.AddForeignKey(
                name: "FK_Facturas_CuentaPorPagars_CuentaPorPagarIdCuentaPorPagar",
                table: "Facturas",
                column: "CuentaPorPagarIdCuentaPorPagar",
                principalTable: "CuentaPorPagars",
                principalColumn: "IdCuentaPorPagar");
        }
    }
}

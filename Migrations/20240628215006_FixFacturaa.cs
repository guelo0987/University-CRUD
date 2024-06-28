using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Migrations
{
    /// <inheritdoc />
    public partial class FixFacturaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facturas_CuentaPorPagars_IdCuentaPorPagar",
                table: "Facturas");

            migrationBuilder.RenameColumn(
                name: "IdCuentaPorPagar",
                table: "Facturas",
                newName: "CuentaPorPagarIdCuentaPorPagar");

            migrationBuilder.RenameIndex(
                name: "IX_Facturas_IdCuentaPorPagar",
                table: "Facturas",
                newName: "IX_Facturas_CuentaPorPagarIdCuentaPorPagar");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaPago",
                table: "Facturas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Facturas_CuentaPorPagars_CuentaPorPagarIdCuentaPorPagar",
                table: "Facturas",
                column: "CuentaPorPagarIdCuentaPorPagar",
                principalTable: "CuentaPorPagars",
                principalColumn: "IdCuentaPorPagar");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facturas_CuentaPorPagars_CuentaPorPagarIdCuentaPorPagar",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "FechaPago",
                table: "Facturas");

            migrationBuilder.RenameColumn(
                name: "CuentaPorPagarIdCuentaPorPagar",
                table: "Facturas",
                newName: "IdCuentaPorPagar");

            migrationBuilder.RenameIndex(
                name: "IX_Facturas_CuentaPorPagarIdCuentaPorPagar",
                table: "Facturas",
                newName: "IX_Facturas_IdCuentaPorPagar");

            migrationBuilder.AddForeignKey(
                name: "FK_Facturas_CuentaPorPagars_IdCuentaPorPagar",
                table: "Facturas",
                column: "IdCuentaPorPagar",
                principalTable: "CuentaPorPagars",
                principalColumn: "IdCuentaPorPagar");
        }
    }
}

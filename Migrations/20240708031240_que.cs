using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Migrations
{
    /// <inheritdoc />
    public partial class que : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facturas_CuentaPorPagars_CuentaPorPagarId",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "PaymentIntentId",
                table: "Facturas");

            migrationBuilder.RenameColumn(
                name: "CuentaPorPagarId",
                table: "Facturas",
                newName: "CuentaPorPagarIdCuentaPorPagar");

            migrationBuilder.RenameIndex(
                name: "IX_Facturas_CuentaPorPagarId",
                table: "Facturas",
                newName: "IX_Facturas_CuentaPorPagarIdCuentaPorPagar");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaPago",
                table: "Facturas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarcaTarjeta",
                table: "Facturas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetodoPago",
                table: "Facturas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeCustomerId",
                table: "Facturas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripePaymentIntentId",
                table: "Facturas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UltimosDigitosTarjeta",
                table: "Facturas",
                type: "nvarchar(max)",
                nullable: true);

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

            migrationBuilder.DropColumn(
                name: "MarcaTarjeta",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "MetodoPago",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "StripeCustomerId",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "StripePaymentIntentId",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "UltimosDigitosTarjeta",
                table: "Facturas");

            migrationBuilder.RenameColumn(
                name: "CuentaPorPagarIdCuentaPorPagar",
                table: "Facturas",
                newName: "CuentaPorPagarId");

            migrationBuilder.RenameIndex(
                name: "IX_Facturas_CuentaPorPagarIdCuentaPorPagar",
                table: "Facturas",
                newName: "IX_Facturas_CuentaPorPagarId");

            migrationBuilder.AddColumn<string>(
                name: "PaymentIntentId",
                table: "Facturas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Facturas_CuentaPorPagars_CuentaPorPagarId",
                table: "Facturas",
                column: "CuentaPorPagarId",
                principalTable: "CuentaPorPagars",
                principalColumn: "IdCuentaPorPagar");
        }
    }
}

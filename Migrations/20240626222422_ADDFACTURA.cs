using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Migrations
{
    /// <inheritdoc />
    public partial class ADDFACTURA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "CuentasPorPagars");

            migrationBuilder.DropColumn(
                name: "FechaPago",
                table: "CuentasPorPagars");

            migrationBuilder.RenameColumn(
                name: "Monto",
                table: "CuentasPorPagars",
                newName: "MontoPorMateria");

            migrationBuilder.CreateTable(
                name: "Facturas",
                columns: table => new
                {
                    FacturaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCuentaPorPagar = table.Column<int>(type: "int", nullable: true),
                    MontoTotalaPagar = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facturas", x => x.FacturaId);
                    table.ForeignKey(
                        name: "FK_Facturas_CuentasPorPagars_IdCuentaPorPagar",
                        column: x => x.IdCuentaPorPagar,
                        principalTable: "CuentasPorPagars",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_IdCuentaPorPagar",
                table: "Facturas",
                column: "IdCuentaPorPagar");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Facturas");

            migrationBuilder.RenameColumn(
                name: "MontoPorMateria",
                table: "CuentasPorPagars",
                newName: "Monto");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "CuentasPorPagars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaPago",
                table: "CuentasPorPagars",
                type: "datetime2",
                nullable: true);
        }
    }
}

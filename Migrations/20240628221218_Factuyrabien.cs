using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Migrations
{
    /// <inheritdoc />
    public partial class Factuyrabien : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MontoTotalaPagar",
                table: "Facturas");

            migrationBuilder.RenameColumn(
                name: "FechaPago",
                table: "Facturas",
                newName: "FechaCreacion");

            migrationBuilder.RenameColumn(
                name: "Estado",
                table: "Facturas",
                newName: "Periodo");

            migrationBuilder.RenameColumn(
                name: "FacturaId",
                table: "Facturas",
                newName: "IdFactura");

            migrationBuilder.AddColumn<int>(
                name: "CodigoEstudiante",
                table: "Facturas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "MontoTotal",
                table: "Facturas",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_CodigoEstudiante",
                table: "Facturas",
                column: "CodigoEstudiante");

            migrationBuilder.AddForeignKey(
                name: "FK_Facturas_Estudiantes_CodigoEstudiante",
                table: "Facturas",
                column: "CodigoEstudiante",
                principalTable: "Estudiantes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facturas_Estudiantes_CodigoEstudiante",
                table: "Facturas");

            migrationBuilder.DropIndex(
                name: "IX_Facturas_CodigoEstudiante",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "CodigoEstudiante",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "MontoTotal",
                table: "Facturas");

            migrationBuilder.RenameColumn(
                name: "Periodo",
                table: "Facturas",
                newName: "Estado");

            migrationBuilder.RenameColumn(
                name: "FechaCreacion",
                table: "Facturas",
                newName: "FechaPago");

            migrationBuilder.RenameColumn(
                name: "IdFactura",
                table: "Facturas",
                newName: "FacturaId");

            migrationBuilder.AddColumn<decimal>(
                name: "MontoTotalaPagar",
                table: "Facturas",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}

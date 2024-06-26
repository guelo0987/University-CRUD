using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Migrations
{
    /// <inheritdoc />
    public partial class FixAddCuentaporPagaar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PeriodoActual",
                table: "EstudianteMaterias",
                newName: "Creditos");

            migrationBuilder.AddColumn<string>(
                name: "PeriodoActual",
                table: "Estudiantes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Periodo",
                table: "EstudianteMaterias",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CuentasPorPagars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoMateria = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CodigoEstudiante = table.Column<int>(type: "int", nullable: true),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaPago = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CuentasPorPagars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CuentasPorPagars_EstudianteMaterias_CodigoMateria_CodigoEstudiante",
                        columns: x => new { x.CodigoMateria, x.CodigoEstudiante },
                        principalTable: "EstudianteMaterias",
                        principalColumns: new[] { "CodigoMateria", "CodigoEstudiante" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_CuentasPorPagars_CodigoMateria_CodigoEstudiante",
                table: "CuentasPorPagars",
                columns: new[] { "CodigoMateria", "CodigoEstudiante" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CuentasPorPagars");

            migrationBuilder.DropColumn(
                name: "PeriodoActual",
                table: "Estudiantes");

            migrationBuilder.DropColumn(
                name: "Periodo",
                table: "EstudianteMaterias");

            migrationBuilder.RenameColumn(
                name: "Creditos",
                table: "EstudianteMaterias",
                newName: "PeriodoActual");
        }
    }
}

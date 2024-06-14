using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carreras",
                columns: table => new
                {
                    CarreraId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCarrera = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DuracionPeriodos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalCreditos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Departamento = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carreras", x => x.CarreraId);
                });

            migrationBuilder.CreateTable(
                name: "Estudiantes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreEstudiante = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DireccionEstudiante = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Nacionalidad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SexoEstudiante = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CiudadEstudiante = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TelefonoEstudiante = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    CorreoEstudiante = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FechaIngreso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PeriodosCursados = table.Column<int>(type: "int", nullable: true),
                    IndiceGeneral = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IndicePeriodo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CondicionAcademica = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreditosAprobados = table.Column<int>(type: "int", nullable: true),
                    CodigoCarrera = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ContraseñaEstudiante = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CarreraId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estudiantes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Estudiantes_Carreras_CarreraId",
                        column: x => x.CarreraId,
                        principalTable: "Carreras",
                        principalColumn: "CarreraId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Estudiantes_CarreraId",
                table: "Estudiantes",
                column: "CarreraId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Estudiantes");

            migrationBuilder.DropTable(
                name: "Carreras");
        }
    }
}

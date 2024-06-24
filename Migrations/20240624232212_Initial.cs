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
                name: "Aulas",
                columns: table => new
                {
                    CodigoAula = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Capacidad = table.Column<int>(type: "int", nullable: false),
                    TipoAula = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aulas", x => x.CodigoAula);
                });

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
                name: "Docentes",
                columns: table => new
                {
                    CodigoDocente = table.Column<int>(type: "int", nullable: false),
                    NombreDocente = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CorreoDocente = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TelefonoDocente = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SexoDocente = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Docentes", x => x.CodigoDocente);
                });

            migrationBuilder.CreateTable(
                name: "Materias",
                columns: table => new
                {
                    CodigoMateria = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NombreMateria = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TipoMateria = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreditosMateria = table.Column<int>(type: "int", nullable: false),
                    AreaAcademica = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materias", x => x.CodigoMateria);
                });

            migrationBuilder.CreateTable(
                name: "Secciones",
                columns: table => new
                {
                    CodigoSeccion = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Horario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Cupo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Secciones", x => x.CodigoSeccion);
                });

            migrationBuilder.CreateTable(
                name: "Estudiantes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
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
                    ContraseñaEstudiante = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CarreraId = table.Column<int>(type: "int", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "CarreraMaterias",
                columns: table => new
                {
                    CarreraId = table.Column<int>(type: "int", nullable: false),
                    CodigoMateria = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarreraMaterias", x => new { x.CarreraId, x.CodigoMateria });
                    table.ForeignKey(
                        name: "FK_CarreraMaterias_Carreras_CarreraId",
                        column: x => x.CarreraId,
                        principalTable: "Carreras",
                        principalColumn: "CarreraId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarreraMaterias_Materias_CodigoMateria",
                        column: x => x.CodigoMateria,
                        principalTable: "Materias",
                        principalColumn: "CodigoMateria",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MateriaAulas",
                columns: table => new
                {
                    AulaId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CodigoMateria = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MateriaAulas", x => new { x.AulaId, x.CodigoMateria });
                    table.ForeignKey(
                        name: "FK_MateriaAulas_Aulas_AulaId",
                        column: x => x.AulaId,
                        principalTable: "Aulas",
                        principalColumn: "CodigoAula",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MateriaAulas_Materias_CodigoMateria",
                        column: x => x.CodigoMateria,
                        principalTable: "Materias",
                        principalColumn: "CodigoMateria",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MateriaDocentes",
                columns: table => new
                {
                    DocenteId = table.Column<int>(type: "int", nullable: false),
                    CodigoMateria = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MateriaDocentes", x => new { x.DocenteId, x.CodigoMateria });
                    table.ForeignKey(
                        name: "FK_MateriaDocentes_Docentes_DocenteId",
                        column: x => x.DocenteId,
                        principalTable: "Docentes",
                        principalColumn: "CodigoDocente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MateriaDocentes_Materias_CodigoMateria",
                        column: x => x.CodigoMateria,
                        principalTable: "Materias",
                        principalColumn: "CodigoMateria",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeccionMaterias",
                columns: table => new
                {
                    CodigoMateria = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CodigoSeccion = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeccionMaterias", x => new { x.CodigoMateria, x.CodigoSeccion });
                    table.ForeignKey(
                        name: "FK_SeccionMaterias_Materias_CodigoMateria",
                        column: x => x.CodigoMateria,
                        principalTable: "Materias",
                        principalColumn: "CodigoMateria",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeccionMaterias_Secciones_CodigoSeccion",
                        column: x => x.CodigoSeccion,
                        principalTable: "Secciones",
                        principalColumn: "CodigoSeccion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EstudianteMaterias",
                columns: table => new
                {
                    CodigoMateria = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CodigoEstudiante = table.Column<int>(type: "int", nullable: false),
                    PeriodoActual = table.Column<int>(type: "int", nullable: false),
                    Calificacion = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstudianteMaterias", x => new { x.CodigoMateria, x.CodigoEstudiante });
                    table.ForeignKey(
                        name: "FK_EstudianteMaterias_Estudiantes_CodigoEstudiante",
                        column: x => x.CodigoEstudiante,
                        principalTable: "Estudiantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EstudianteMaterias_Materias_CodigoMateria",
                        column: x => x.CodigoMateria,
                        principalTable: "Materias",
                        principalColumn: "CodigoMateria",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarreraMaterias_CodigoMateria",
                table: "CarreraMaterias",
                column: "CodigoMateria");

            migrationBuilder.CreateIndex(
                name: "IX_EstudianteMaterias_CodigoEstudiante",
                table: "EstudianteMaterias",
                column: "CodigoEstudiante");

            migrationBuilder.CreateIndex(
                name: "IX_Estudiantes_CarreraId",
                table: "Estudiantes",
                column: "CarreraId");

            migrationBuilder.CreateIndex(
                name: "IX_MateriaAulas_CodigoMateria",
                table: "MateriaAulas",
                column: "CodigoMateria");

            migrationBuilder.CreateIndex(
                name: "IX_MateriaDocentes_CodigoMateria",
                table: "MateriaDocentes",
                column: "CodigoMateria");

            migrationBuilder.CreateIndex(
                name: "IX_SeccionMaterias_CodigoSeccion",
                table: "SeccionMaterias",
                column: "CodigoSeccion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarreraMaterias");

            migrationBuilder.DropTable(
                name: "EstudianteMaterias");

            migrationBuilder.DropTable(
                name: "MateriaAulas");

            migrationBuilder.DropTable(
                name: "MateriaDocentes");

            migrationBuilder.DropTable(
                name: "SeccionMaterias");

            migrationBuilder.DropTable(
                name: "Estudiantes");

            migrationBuilder.DropTable(
                name: "Aulas");

            migrationBuilder.DropTable(
                name: "Docentes");

            migrationBuilder.DropTable(
                name: "Materias");

            migrationBuilder.DropTable(
                name: "Secciones");

            migrationBuilder.DropTable(
                name: "Carreras");
        }
    }
}

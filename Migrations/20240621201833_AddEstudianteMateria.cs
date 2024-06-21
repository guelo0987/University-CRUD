using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Migrations
{
    /// <inheritdoc />
    public partial class AddEstudianteMateria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstudianteMaterias",
                columns: table => new
                {
                    CodigoMateria = table.Column<int>(type: "int", nullable: false),
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
                name: "IX_EstudianteMaterias_CodigoEstudiante",
                table: "EstudianteMaterias",
                column: "CodigoEstudiante");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstudianteMaterias");
        }
    }
}

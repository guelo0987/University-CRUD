using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMateriaAula : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MateriaAulas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MateriaAulas",
                columns: table => new
                {
                    AulaId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CodigoMateria = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SeccionId = table.Column<string>(type: "nvarchar(450)", nullable: true)
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
                    table.ForeignKey(
                        name: "FK_MateriaAulas_Secciones_SeccionId",
                        column: x => x.SeccionId,
                        principalTable: "Secciones",
                        principalColumn: "CodigoSeccion");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MateriaAulas_CodigoMateria",
                table: "MateriaAulas",
                column: "CodigoMateria");

            migrationBuilder.CreateIndex(
                name: "IX_MateriaAulas_SeccionId",
                table: "MateriaAulas",
                column: "SeccionId");
        }
    }
}

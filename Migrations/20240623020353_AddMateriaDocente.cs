using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Migrations
{
    /// <inheritdoc />
    public partial class AddMateriaDocente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Docentes",
                columns: table => new
                {
                    CodigoDocente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                name: "MateriaDocentes",
                columns: table => new
                {
                    DocenteId = table.Column<int>(type: "int", nullable: false),
                    CodigoMateria = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_MateriaDocentes_CodigoMateria",
                table: "MateriaDocentes",
                column: "CodigoMateria");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MateriaDocentes");

            migrationBuilder.DropTable(
                name: "Docentes");
        }
    }
}

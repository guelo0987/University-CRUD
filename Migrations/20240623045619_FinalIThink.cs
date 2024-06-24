using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Migrations
{
    /// <inheritdoc />
    public partial class FinalIThink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Secciones_Materias_CodigoMateria",
                table: "Secciones");

            migrationBuilder.DropIndex(
                name: "IX_Secciones_CodigoMateria",
                table: "Secciones");

            migrationBuilder.DropColumn(
                name: "CodigoMateria",
                table: "Secciones");

            migrationBuilder.CreateTable(
                name: "SeccionMaterias",
                columns: table => new
                {
                    CodigoMateria = table.Column<int>(type: "int", nullable: false),
                    CodigoSeccion = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_SeccionMaterias_CodigoSeccion",
                table: "SeccionMaterias",
                column: "CodigoSeccion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeccionMaterias");

            migrationBuilder.AddColumn<int>(
                name: "CodigoMateria",
                table: "Secciones",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Secciones_CodigoMateria",
                table: "Secciones",
                column: "CodigoMateria");

            migrationBuilder.AddForeignKey(
                name: "FK_Secciones_Materias_CodigoMateria",
                table: "Secciones",
                column: "CodigoMateria",
                principalTable: "Materias",
                principalColumn: "CodigoMateria");
        }
    }
}

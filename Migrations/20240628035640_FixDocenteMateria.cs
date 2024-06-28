using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Migrations
{
    /// <inheritdoc />
    public partial class FixDocenteMateria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SeccionId",
                table: "MateriaDocentes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MateriaDocentes_SeccionId",
                table: "MateriaDocentes",
                column: "SeccionId");

            migrationBuilder.AddForeignKey(
                name: "FK_MateriaDocentes_Secciones_SeccionId",
                table: "MateriaDocentes",
                column: "SeccionId",
                principalTable: "Secciones",
                principalColumn: "CodigoSeccion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MateriaDocentes_Secciones_SeccionId",
                table: "MateriaDocentes");

            migrationBuilder.DropIndex(
                name: "IX_MateriaDocentes_SeccionId",
                table: "MateriaDocentes");

            migrationBuilder.DropColumn(
                name: "SeccionId",
                table: "MateriaDocentes");
        }
    }
}

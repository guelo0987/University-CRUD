using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Migrations
{
    /// <inheritdoc />
    public partial class Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoAula",
                table: "Secciones",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AulaCodigoAula",
                table: "Materias",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeccionId",
                table: "MateriaAulas",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Secciones_CodigoAula",
                table: "Secciones",
                column: "CodigoAula");

            migrationBuilder.CreateIndex(
                name: "IX_Materias_AulaCodigoAula",
                table: "Materias",
                column: "AulaCodigoAula");

            migrationBuilder.CreateIndex(
                name: "IX_MateriaAulas_SeccionId",
                table: "MateriaAulas",
                column: "SeccionId");

            migrationBuilder.AddForeignKey(
                name: "FK_MateriaAulas_Secciones_SeccionId",
                table: "MateriaAulas",
                column: "SeccionId",
                principalTable: "Secciones",
                principalColumn: "CodigoSeccion");

            migrationBuilder.AddForeignKey(
                name: "FK_Materias_Aulas_AulaCodigoAula",
                table: "Materias",
                column: "AulaCodigoAula",
                principalTable: "Aulas",
                principalColumn: "CodigoAula");

            migrationBuilder.AddForeignKey(
                name: "FK_Secciones_Aulas_CodigoAula",
                table: "Secciones",
                column: "CodigoAula",
                principalTable: "Aulas",
                principalColumn: "CodigoAula");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MateriaAulas_Secciones_SeccionId",
                table: "MateriaAulas");

            migrationBuilder.DropForeignKey(
                name: "FK_Materias_Aulas_AulaCodigoAula",
                table: "Materias");

            migrationBuilder.DropForeignKey(
                name: "FK_Secciones_Aulas_CodigoAula",
                table: "Secciones");

            migrationBuilder.DropIndex(
                name: "IX_Secciones_CodigoAula",
                table: "Secciones");

            migrationBuilder.DropIndex(
                name: "IX_Materias_AulaCodigoAula",
                table: "Materias");

            migrationBuilder.DropIndex(
                name: "IX_MateriaAulas_SeccionId",
                table: "MateriaAulas");

            migrationBuilder.DropColumn(
                name: "CodigoAula",
                table: "Secciones");

            migrationBuilder.DropColumn(
                name: "AulaCodigoAula",
                table: "Materias");

            migrationBuilder.DropColumn(
                name: "SeccionId",
                table: "MateriaAulas");
        }
    }
}

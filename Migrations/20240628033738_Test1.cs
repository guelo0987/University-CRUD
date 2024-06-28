using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Migrations
{
    /// <inheritdoc />
    public partial class Test1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SeccionId",
                table: "EstudianteMaterias",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EstudianteMaterias_SeccionId",
                table: "EstudianteMaterias",
                column: "SeccionId");

            migrationBuilder.AddForeignKey(
                name: "FK_EstudianteMaterias_Secciones_SeccionId",
                table: "EstudianteMaterias",
                column: "SeccionId",
                principalTable: "Secciones",
                principalColumn: "CodigoSeccion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EstudianteMaterias_Secciones_SeccionId",
                table: "EstudianteMaterias");

            migrationBuilder.DropIndex(
                name: "IX_EstudianteMaterias_SeccionId",
                table: "EstudianteMaterias");

            migrationBuilder.DropColumn(
                name: "SeccionId",
                table: "EstudianteMaterias");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Migrations
{
    /// <inheritdoc />
    public partial class calificacionfinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Calificacion",
                table: "EstudianteMaterias",
                newName: "CalificacionFinal");

            migrationBuilder.AddColumn<string>(
                name: "CalificacionMedioTermino",
                table: "EstudianteMaterias",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CalificacionMedioTermino",
                table: "EstudianteMaterias");

            migrationBuilder.RenameColumn(
                name: "CalificacionFinal",
                table: "EstudianteMaterias",
                newName: "Calificacion");
        }
    }
}

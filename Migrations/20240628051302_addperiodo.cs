using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Migrations
{
    /// <inheritdoc />
    public partial class addperiodo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Calificacion",
                table: "CarreraMaterias");

            migrationBuilder.DropColumn(
                name: "PeriodoCursado",
                table: "CarreraMaterias");

            migrationBuilder.AddColumn<string>(
                name: "PeriodoCursado",
                table: "EstudianteMaterias",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EstudianteId",
                table: "CarreraMaterias",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarreraMaterias_EstudianteId",
                table: "CarreraMaterias",
                column: "EstudianteId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarreraMaterias_Estudiantes_EstudianteId",
                table: "CarreraMaterias",
                column: "EstudianteId",
                principalTable: "Estudiantes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarreraMaterias_Estudiantes_EstudianteId",
                table: "CarreraMaterias");

            migrationBuilder.DropIndex(
                name: "IX_CarreraMaterias_EstudianteId",
                table: "CarreraMaterias");

            migrationBuilder.DropColumn(
                name: "PeriodoCursado",
                table: "EstudianteMaterias");

            migrationBuilder.DropColumn(
                name: "EstudianteId",
                table: "CarreraMaterias");

            migrationBuilder.AddColumn<string>(
                name: "Calificacion",
                table: "CarreraMaterias",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PeriodoCursado",
                table: "CarreraMaterias",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

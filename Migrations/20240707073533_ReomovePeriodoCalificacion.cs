using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Migrations
{
    /// <inheritdoc />
    public partial class ReomovePeriodoCalificacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PeriodoCalificacion",
                table: "EstudianteMaterias");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PeriodoCalificacion",
                table: "EstudianteMaterias",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

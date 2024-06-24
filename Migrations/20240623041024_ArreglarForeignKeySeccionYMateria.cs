using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Migrations
{
    /// <inheritdoc />
    public partial class ArreglarForeignKeySeccionYMateria : Migration
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

            migrationBuilder.AddColumn<int>(
                name: "CodigoSeccion",
                table: "Materias",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Materias_CodigoSeccion",
                table: "Materias",
                column: "CodigoSeccion");

            migrationBuilder.AddForeignKey(
                name: "FK_Materias_Secciones_CodigoSeccion",
                table: "Materias",
                column: "CodigoSeccion",
                principalTable: "Secciones",
                principalColumn: "CodigoSeccion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materias_Secciones_CodigoSeccion",
                table: "Materias");

            migrationBuilder.DropIndex(
                name: "IX_Materias_CodigoSeccion",
                table: "Materias");

            migrationBuilder.DropColumn(
                name: "CodigoSeccion",
                table: "Materias");

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

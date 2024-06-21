using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Migrations
{
    /// <inheritdoc />
    public partial class makesure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarreraMateria_Carreras_CarreraId",
                table: "CarreraMateria");

            migrationBuilder.DropForeignKey(
                name: "FK_CarreraMateria_Materia_CodigoMateria",
                table: "CarreraMateria");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Materia",
                table: "Materia");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarreraMateria",
                table: "CarreraMateria");

            migrationBuilder.RenameTable(
                name: "Materia",
                newName: "Materias");

            migrationBuilder.RenameTable(
                name: "CarreraMateria",
                newName: "CarreraMaterias");

            migrationBuilder.RenameIndex(
                name: "IX_CarreraMateria_CodigoMateria",
                table: "CarreraMaterias",
                newName: "IX_CarreraMaterias_CodigoMateria");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Materias",
                table: "Materias",
                column: "CodigoMateria");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarreraMaterias",
                table: "CarreraMaterias",
                columns: new[] { "CarreraId", "CodigoMateria" });

            migrationBuilder.AddForeignKey(
                name: "FK_CarreraMaterias_Carreras_CarreraId",
                table: "CarreraMaterias",
                column: "CarreraId",
                principalTable: "Carreras",
                principalColumn: "CarreraId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarreraMaterias_Materias_CodigoMateria",
                table: "CarreraMaterias",
                column: "CodigoMateria",
                principalTable: "Materias",
                principalColumn: "CodigoMateria",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarreraMaterias_Carreras_CarreraId",
                table: "CarreraMaterias");

            migrationBuilder.DropForeignKey(
                name: "FK_CarreraMaterias_Materias_CodigoMateria",
                table: "CarreraMaterias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Materias",
                table: "Materias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarreraMaterias",
                table: "CarreraMaterias");

            migrationBuilder.RenameTable(
                name: "Materias",
                newName: "Materia");

            migrationBuilder.RenameTable(
                name: "CarreraMaterias",
                newName: "CarreraMateria");

            migrationBuilder.RenameIndex(
                name: "IX_CarreraMaterias_CodigoMateria",
                table: "CarreraMateria",
                newName: "IX_CarreraMateria_CodigoMateria");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Materia",
                table: "Materia",
                column: "CodigoMateria");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarreraMateria",
                table: "CarreraMateria",
                columns: new[] { "CarreraId", "CodigoMateria" });

            migrationBuilder.AddForeignKey(
                name: "FK_CarreraMateria_Carreras_CarreraId",
                table: "CarreraMateria",
                column: "CarreraId",
                principalTable: "Carreras",
                principalColumn: "CarreraId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarreraMateria_Materia_CodigoMateria",
                table: "CarreraMateria",
                column: "CodigoMateria",
                principalTable: "Materia",
                principalColumn: "CodigoMateria",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

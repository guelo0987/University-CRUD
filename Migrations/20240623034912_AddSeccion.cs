using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Migrations
{
    /// <inheritdoc />
    public partial class AddSeccion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Secciones",
                columns: table => new
                {
                    CodigoSeccion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoMateria = table.Column<int>(type: "int", nullable: true),
                    Horario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Cupo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Secciones", x => x.CodigoSeccion);
                    table.ForeignKey(
                        name: "FK_Secciones_Materias_CodigoMateria",
                        column: x => x.CodigoMateria,
                        principalTable: "Materias",
                        principalColumn: "CodigoMateria");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Secciones_CodigoMateria",
                table: "Secciones",
                column: "CodigoMateria");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Secciones");
        }
    }
}

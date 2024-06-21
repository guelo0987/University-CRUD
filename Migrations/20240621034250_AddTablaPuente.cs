using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Migrations
{
    /// <inheritdoc />
    public partial class AddTablaPuente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Materia",
                columns: table => new
                {
                    CodigoMateria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreMateria = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TipoMateria = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreditosMateria = table.Column<int>(type: "int", nullable: false),
                    AreaAcademica = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materia", x => x.CodigoMateria);
                });

            migrationBuilder.CreateTable(
                name: "CarreraMateria",
                columns: table => new
                {
                    CarreraId = table.Column<int>(type: "int", nullable: false),
                    CodigoMateria = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarreraMateria", x => new { x.CarreraId, x.CodigoMateria });
                    table.ForeignKey(
                        name: "FK_CarreraMateria_Carreras_CarreraId",
                        column: x => x.CarreraId,
                        principalTable: "Carreras",
                        principalColumn: "CarreraId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarreraMateria_Materia_CodigoMateria",
                        column: x => x.CodigoMateria,
                        principalTable: "Materia",
                        principalColumn: "CodigoMateria",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarreraMateria_CodigoMateria",
                table: "CarreraMateria",
                column: "CodigoMateria");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarreraMateria");

            migrationBuilder.DropTable(
                name: "Materia");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Migrations
{
    /// <inheritdoc />
    public partial class FixSeccion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Facturas");

            migrationBuilder.DropTable(
                name: "SeccionMaterias");

            migrationBuilder.DropTable(
                name: "CuentasPorPagars");

            migrationBuilder.DropColumn(
                name: "Periodo",
                table: "EstudianteMaterias");

            migrationBuilder.AddColumn<string>(
                name: "CodigoMateria",
                table: "Secciones",
                type: "nvarchar(450)",
                nullable: true);

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

            migrationBuilder.AlterColumn<string>(
                name: "TipoAula",
                table: "Aulas",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(6)",
                oldMaxLength: 6);

            migrationBuilder.AlterColumn<int>(
                name: "Capacidad",
                table: "Aulas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Calificacion",
                table: "CarreraMaterias");

            migrationBuilder.DropColumn(
                name: "PeriodoCursado",
                table: "CarreraMaterias");

            migrationBuilder.AddColumn<string>(
                name: "Periodo",
                table: "EstudianteMaterias",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TipoAula",
                table: "Aulas",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(6)",
                oldMaxLength: 6,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Capacidad",
                table: "Aulas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "CuentasPorPagars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoMateria = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CodigoEstudiante = table.Column<int>(type: "int", nullable: true),
                    MontoPorMateria = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CuentasPorPagars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CuentasPorPagars_EstudianteMaterias_CodigoMateria_CodigoEstudiante",
                        columns: x => new { x.CodigoMateria, x.CodigoEstudiante },
                        principalTable: "EstudianteMaterias",
                        principalColumns: new[] { "CodigoMateria", "CodigoEstudiante" });
                });

            migrationBuilder.CreateTable(
                name: "SeccionMaterias",
                columns: table => new
                {
                    CodigoMateria = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CodigoSeccion = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeccionMaterias", x => new { x.CodigoMateria, x.CodigoSeccion });
                    table.ForeignKey(
                        name: "FK_SeccionMaterias_Materias_CodigoMateria",
                        column: x => x.CodigoMateria,
                        principalTable: "Materias",
                        principalColumn: "CodigoMateria",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeccionMaterias_Secciones_CodigoSeccion",
                        column: x => x.CodigoSeccion,
                        principalTable: "Secciones",
                        principalColumn: "CodigoSeccion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Facturas",
                columns: table => new
                {
                    FacturaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCuentaPorPagar = table.Column<int>(type: "int", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MontoTotalaPagar = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facturas", x => x.FacturaId);
                    table.ForeignKey(
                        name: "FK_Facturas_CuentasPorPagars_IdCuentaPorPagar",
                        column: x => x.IdCuentaPorPagar,
                        principalTable: "CuentasPorPagars",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CuentasPorPagars_CodigoMateria_CodigoEstudiante",
                table: "CuentasPorPagars",
                columns: new[] { "CodigoMateria", "CodigoEstudiante" });

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_IdCuentaPorPagar",
                table: "Facturas",
                column: "IdCuentaPorPagar");

            migrationBuilder.CreateIndex(
                name: "IX_SeccionMaterias_CodigoSeccion",
                table: "SeccionMaterias",
                column: "CodigoSeccion");
        }
    }
}

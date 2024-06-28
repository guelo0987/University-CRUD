using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Migrations
{
    /// <inheritdoc />
    public partial class VamoAve : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CuentaPorPagars",
                columns: table => new
                {
                    IdCuentaPorPagar = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoMateria = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CodigoEstudiante = table.Column<int>(type: "int", nullable: false),
                    MontoTotalaPagar = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CuentaPorPagars", x => x.IdCuentaPorPagar);
                    table.ForeignKey(
                        name: "FK_CuentaPorPagars_EstudianteMaterias_CodigoMateria_CodigoEstudiante",
                        columns: x => new { x.CodigoMateria, x.CodigoEstudiante },
                        principalTable: "EstudianteMaterias",
                        principalColumns: new[] { "CodigoMateria", "CodigoEstudiante" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Facturas",
                columns: table => new
                {
                    FacturaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCuentaPorPagar = table.Column<int>(type: "int", nullable: true),
                    MontoTotalaPagar = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facturas", x => x.FacturaId);
                    table.ForeignKey(
                        name: "FK_Facturas_CuentaPorPagars_IdCuentaPorPagar",
                        column: x => x.IdCuentaPorPagar,
                        principalTable: "CuentaPorPagars",
                        principalColumn: "IdCuentaPorPagar");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CuentaPorPagars_CodigoMateria_CodigoEstudiante",
                table: "CuentaPorPagars",
                columns: new[] { "CodigoMateria", "CodigoEstudiante" });

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_IdCuentaPorPagar",
                table: "Facturas",
                column: "IdCuentaPorPagar");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Facturas");

            migrationBuilder.DropTable(
                name: "CuentaPorPagars");
        }
    }
}

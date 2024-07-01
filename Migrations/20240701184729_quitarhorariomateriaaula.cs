using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Migrations
{
    /// <inheritdoc />
    public partial class quitarhorariomateriaaula : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Horario",
                table: "MateriaAulas");

            migrationBuilder.AddColumn<string>(
                name: "Edificio",
                table: "Aulas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Edificio",
                table: "Aulas");

            migrationBuilder.AddColumn<string>(
                name: "Horario",
                table: "MateriaAulas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

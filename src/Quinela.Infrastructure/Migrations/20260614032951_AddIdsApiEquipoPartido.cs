using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quinela.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIdsApiEquipoPartido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Ids del API externo (worldcup26.ir) para sincronizar resultados.
            migrationBuilder.AddColumn<string>(
                name: "partido_id_api",
                table: "partidos",
                type: "character varying(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "equipo_id_api",
                table: "equipos",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "equipo_id_api_largo",
                table: "equipos",
                type: "character varying(40)",
                maxLength: 40,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "partido_id_api",
                table: "partidos");

            migrationBuilder.DropColumn(
                name: "equipo_id_api",
                table: "equipos");

            migrationBuilder.DropColumn(
                name: "equipo_id_api_largo",
                table: "equipos");
        }
    }
}

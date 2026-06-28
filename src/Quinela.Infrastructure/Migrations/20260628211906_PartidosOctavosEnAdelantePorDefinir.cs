using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quinela.Infrastructure.Migrations
{
    /// <summary>
    /// De octavos en adelante los partidos de eliminatoria son "por definir" (sus participantes
    /// salen del árbol). Los dieciseisavos NO: sus equipos se resuelven con las posiciones de grupo.
    /// Se decide por tipo_partido_id (Dieciseisavos=2, Octavos=3, Cuartos=4, Semifinal=5, Tercer=6, Final=7).
    /// </summary>
    public partial class PartidosOctavosEnAdelantePorDefinir : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Dieciseisavos: salen de las posiciones de grupo -> no son "por definir".
            migrationBuilder.Sql(
                "UPDATE partidos SET por_definirse = FALSE, updated_at = now(), updated_by = 'migration' WHERE fase_id = 2 AND tipo_partido_id = 2;");

            // Octavos en adelante: salen del árbol -> "por definir".
            migrationBuilder.Sql(
                "UPDATE partidos SET por_definirse = TRUE, updated_at = now(), updated_by = 'migration' WHERE fase_id = 2 AND tipo_partido_id >= 3;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Estado anterior: toda la eliminatoria estaba como "por definir".
            migrationBuilder.Sql(
                "UPDATE partidos SET por_definirse = TRUE WHERE fase_id = 2;");
        }
    }
}

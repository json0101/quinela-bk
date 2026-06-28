using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Quinela.Infrastructure.Migrations
{
    /// <summary>
    /// Tipos de partido de la eliminatoria (fase 2), con sus puntos de quiniela (exacto/acertado)
    /// y los mismos puntos de partido que la fase de grupos (victoria 3 / empate 1) para la tabla
    /// general de la FIFA. Además se corrige el tipo_partido_id de los partidos 73-104 según su ronda.
    /// </summary>
    public partial class SeedTiposPartidoEliminatoria : Migration
    {
        private static readonly DateTime Seed = new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc);

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "tipos_partido",
                columns: new[]
                {
                    "id", "descripcion", "fase_id", "pts_partido_victoria", "pts_partido_empate",
                    "pts_quinela_resultado_exacto", "pts_quinela_resultado_acertado", "created_at", "created_by", "active"
                },
                values: new object[,]
                {
                    // victoria 3 / empate 1 (igual que la fase de grupos, para la tabla general).
                    { 2, "Dieciseisavos", 2, 3, 1, 6, 2, Seed, "seed", true },
                    { 3, "Octavos", 2, 3, 1, 9, 3, Seed, "seed", true },
                    { 4, "Cuartos", 2, 3, 1, 12, 4, Seed, "seed", true },
                    { 5, "Semifinal", 2, 3, 1, 15, 5, Seed, "seed", true },
                    { 6, "Tercer lugar", 2, 3, 1, 18, 6, Seed, "seed", true },
                    { 7, "Final", 2, 3, 1, 21, 7, Seed, "seed", true }
                });

            // Asigna a cada partido de eliminatoria el tipo según su ronda (Id = nº de partido FIFA).
            migrationBuilder.Sql("UPDATE partidos SET tipo_partido_id = 2 WHERE fase_id = 2 AND id BETWEEN 73 AND 88;");   // Dieciseisavos
            migrationBuilder.Sql("UPDATE partidos SET tipo_partido_id = 3 WHERE fase_id = 2 AND id BETWEEN 89 AND 96;");   // Octavos
            migrationBuilder.Sql("UPDATE partidos SET tipo_partido_id = 4 WHERE fase_id = 2 AND id BETWEEN 97 AND 100;");  // Cuartos
            migrationBuilder.Sql("UPDATE partidos SET tipo_partido_id = 5 WHERE fase_id = 2 AND id IN (101, 102);");        // Semifinal
            migrationBuilder.Sql("UPDATE partidos SET tipo_partido_id = 6 WHERE fase_id = 2 AND id = 103;");                // Tercer lugar
            migrationBuilder.Sql("UPDATE partidos SET tipo_partido_id = 7 WHERE fase_id = 2 AND id = 104;");                // Final
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Devuelve los partidos de eliminatoria al tipo base antes de borrar los tipos nuevos.
            migrationBuilder.Sql("UPDATE partidos SET tipo_partido_id = 1 WHERE fase_id = 2;");

            for (var id = 7; id >= 2; id--)
                migrationBuilder.DeleteData(table: "tipos_partido", keyColumn: "id", keyValue: id);
        }
    }
}

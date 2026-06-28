using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Quinela.Infrastructure.Migrations
{
    /// <summary>
    /// Crea los 32 partidos de la eliminatoria del Mundial 2026 (fase 2), con Id = número de
    /// partido FIFA (73-104) y el árbol cableado en partido_ganador_local_id / partido_ganador_visitante_id.
    /// Los equipos arrancan como placeholder (1 y 2): el servicio DistributionEliminatoryWorldCup2026
    /// los resuelve según las posiciones de grupo y propaga los ganadores. El 3er puesto (103) deja el
    /// árbol nulo porque sus participantes son los PERDEDORES de las semifinales (pendiente de modelar).
    /// </summary>
    public partial class AddPartidosEliminatoriaMundial2026 : Migration
    {
        private static readonly DateTime Seed = new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc);

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Orden ascendente por id: los partidos que alimentan (ids menores) se insertan antes
            // de los que los referencian, para respetar la auto-FK del árbol.
            migrationBuilder.InsertData(
                table: "partidos",
                columns: new[]
                {
                    "id", "fecha_partido", "torneo_id", "grupo_id", "fase_id", "equipo_local_id",
                    "equipo_visitante_id", "tipo_partido_id", "estado", "aplica_definicion_penales",
                    "partido_ganador_local_id", "partido_ganador_visitante_id", "created_at", "created_by", "active"
                },
                values: new object[,]
                {
                    // El partido #73 (2B vs 2A) NO se siembra: se crea manualmente en el servidor
                    // para evitar duplicado. El octavos #90 deja su feeder local en null (re-enlazar
                    // partido_ganador_local_id de #90 al id del partido manual).
                    { 74, new DateTime(2026, 6, 29, 18, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2, 1, 2, 1, 'P', true, null, null, Seed, "seed", true },
                    { 75, new DateTime(2026, 6, 29, 21, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2, 1, 2, 1, 'P', true, null, null, Seed, "seed", true },
                    { 76, new DateTime(2026, 6, 29, 23, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2, 1, 2, 1, 'P', true, null, null, Seed, "seed", true },
                    { 77, new DateTime(2026, 6, 30, 18, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2, 1, 2, 1, 'P', true, null, null, Seed, "seed", true },
                    { 78, new DateTime(2026, 6, 30, 21, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2, 1, 2, 1, 'P', true, null, null, Seed, "seed", true },
                    { 79, new DateTime(2026, 6, 30, 23, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2, 1, 2, 1, 'P', true, null, null, Seed, "seed", true },
                    { 80, new DateTime(2026, 7, 1, 18, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2, 1, 2, 1, 'P', true, null, null, Seed, "seed", true },
                    { 81, new DateTime(2026, 7, 1, 21, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2, 1, 2, 1, 'P', true, null, null, Seed, "seed", true },
                    { 82, new DateTime(2026, 7, 1, 23, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2, 1, 2, 1, 'P', true, null, null, Seed, "seed", true },
                    { 83, new DateTime(2026, 7, 2, 18, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2, 1, 2, 1, 'P', true, null, null, Seed, "seed", true },
                    { 84, new DateTime(2026, 7, 2, 21, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2, 1, 2, 1, 'P', true, null, null, Seed, "seed", true },
                    { 85, new DateTime(2026, 7, 2, 23, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2, 1, 2, 1, 'P', true, null, null, Seed, "seed", true },
                    { 86, new DateTime(2026, 7, 3, 18, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2, 1, 2, 1, 'P', true, null, null, Seed, "seed", true },
                    { 87, new DateTime(2026, 7, 3, 21, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2, 1, 2, 1, 'P', true, null, null, Seed, "seed", true },
                    { 88, new DateTime(2026, 7, 3, 23, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2, 1, 2, 1, 'P', true, null, null, Seed, "seed", true },
                    { 89, new DateTime(2026, 7, 4, 18, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2, 1, 2, 1, 'P', true, 74, 77, Seed, "seed", true },
                    { 90, new DateTime(2026, 7, 4, 21, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2, 1, 2, 1, 'P', true, null, 75, Seed, "seed", true },
                    { 91, new DateTime(2026, 7, 5, 18, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2, 1, 2, 1, 'P', true, 76, 78, Seed, "seed", true },
                    { 92, new DateTime(2026, 7, 5, 21, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2, 1, 2, 1, 'P', true, 79, 80, Seed, "seed", true },
                    { 93, new DateTime(2026, 7, 6, 18, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2, 1, 2, 1, 'P', true, 83, 84, Seed, "seed", true },
                    { 94, new DateTime(2026, 7, 6, 21, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2, 1, 2, 1, 'P', true, 81, 82, Seed, "seed", true },
                    { 95, new DateTime(2026, 7, 7, 18, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2, 1, 2, 1, 'P', true, 86, 88, Seed, "seed", true },
                    { 96, new DateTime(2026, 7, 7, 21, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2, 1, 2, 1, 'P', true, 85, 87, Seed, "seed", true },
                    { 97, new DateTime(2026, 7, 9, 21, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2, 1, 2, 1, 'P', true, 89, 90, Seed, "seed", true },
                    { 98, new DateTime(2026, 7, 10, 21, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2, 1, 2, 1, 'P', true, 93, 94, Seed, "seed", true },
                    { 99, new DateTime(2026, 7, 11, 18, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2, 1, 2, 1, 'P', true, 91, 92, Seed, "seed", true },
                    { 100, new DateTime(2026, 7, 11, 21, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2, 1, 2, 1, 'P', true, 95, 96, Seed, "seed", true },
                    { 101, new DateTime(2026, 7, 14, 21, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2, 1, 2, 1, 'P', true, 97, 98, Seed, "seed", true },
                    { 102, new DateTime(2026, 7, 15, 21, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2, 1, 2, 1, 'P', true, 99, 100, Seed, "seed", true },
                    { 103, new DateTime(2026, 7, 18, 18, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2, 1, 2, 1, 'P', true, null, null, Seed, "seed", true },
                    { 104, new DateTime(2026, 7, 19, 18, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2, 1, 2, 1, 'P', true, 101, 102, Seed, "seed", true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Orden descendente: se borran primero los que referencian (ids mayores).
            // El #73 no se siembra (se crea manual), así que el rango va de 104 a 74.
            for (var id = 104; id >= 74; id--)
                migrationBuilder.DeleteData(table: "partidos", keyColumn: "id", keyValue: id);
        }
    }
}

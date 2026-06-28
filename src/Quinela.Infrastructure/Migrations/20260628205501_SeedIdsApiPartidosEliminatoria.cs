using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quinela.Infrastructure.Migrations
{
    /// <summary>
    /// Asigna el partido_id_api (worldcup26.ir, campo _id) a cada partido de eliminatoria.
    /// El mapeo es directo: el campo "id" del juego en el API es el número de partido FIFA,
    /// que coincide con el Id de nuestros partidos de eliminatoria (73-104). El #73 no existe
    /// en la BD (se crea manual), así que ese UPDATE es no-op.
    /// </summary>
    public partial class SeedIdsApiPartidosEliminatoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE partidos SET partido_id_api = '679c9c8a5749c4077500e073', updated_at = now(), updated_by = 'sync-api' WHERE id = 73 AND fase_id = 2;");
            migrationBuilder.Sql("UPDATE partidos SET partido_id_api = '679c9c8a5749c4077500e074', updated_at = now(), updated_by = 'sync-api' WHERE id = 74 AND fase_id = 2;");
            migrationBuilder.Sql("UPDATE partidos SET partido_id_api = '679c9c8a5749c4077500e075', updated_at = now(), updated_by = 'sync-api' WHERE id = 75 AND fase_id = 2;");
            migrationBuilder.Sql("UPDATE partidos SET partido_id_api = '679c9c8a5749c4077500e076', updated_at = now(), updated_by = 'sync-api' WHERE id = 76 AND fase_id = 2;");
            migrationBuilder.Sql("UPDATE partidos SET partido_id_api = '679c9c8a5749c4077500e077', updated_at = now(), updated_by = 'sync-api' WHERE id = 77 AND fase_id = 2;");
            migrationBuilder.Sql("UPDATE partidos SET partido_id_api = '679c9c8a5749c4077500e078', updated_at = now(), updated_by = 'sync-api' WHERE id = 78 AND fase_id = 2;");
            migrationBuilder.Sql("UPDATE partidos SET partido_id_api = '679c9c8a5749c4077500e079', updated_at = now(), updated_by = 'sync-api' WHERE id = 79 AND fase_id = 2;");
            migrationBuilder.Sql("UPDATE partidos SET partido_id_api = '679c9c8a5749c4077500e07a', updated_at = now(), updated_by = 'sync-api' WHERE id = 80 AND fase_id = 2;");
            migrationBuilder.Sql("UPDATE partidos SET partido_id_api = '679c9c8a5749c4077500e07b', updated_at = now(), updated_by = 'sync-api' WHERE id = 81 AND fase_id = 2;");
            migrationBuilder.Sql("UPDATE partidos SET partido_id_api = '679c9c8a5749c4077500e07c', updated_at = now(), updated_by = 'sync-api' WHERE id = 82 AND fase_id = 2;");
            migrationBuilder.Sql("UPDATE partidos SET partido_id_api = '679c9c8a5749c4077500e07d', updated_at = now(), updated_by = 'sync-api' WHERE id = 83 AND fase_id = 2;");
            migrationBuilder.Sql("UPDATE partidos SET partido_id_api = '679c9c8a5749c4077500e07e', updated_at = now(), updated_by = 'sync-api' WHERE id = 84 AND fase_id = 2;");
            migrationBuilder.Sql("UPDATE partidos SET partido_id_api = '679c9c8a5749c4077500e07f', updated_at = now(), updated_by = 'sync-api' WHERE id = 85 AND fase_id = 2;");
            migrationBuilder.Sql("UPDATE partidos SET partido_id_api = '679c9c8a5749c4077500e080', updated_at = now(), updated_by = 'sync-api' WHERE id = 86 AND fase_id = 2;");
            migrationBuilder.Sql("UPDATE partidos SET partido_id_api = '679c9c8a5749c4077500e081', updated_at = now(), updated_by = 'sync-api' WHERE id = 87 AND fase_id = 2;");
            migrationBuilder.Sql("UPDATE partidos SET partido_id_api = '679c9c8a5749c4077500e082', updated_at = now(), updated_by = 'sync-api' WHERE id = 88 AND fase_id = 2;");
            migrationBuilder.Sql("UPDATE partidos SET partido_id_api = '679c9c8a5749c4077500e083', updated_at = now(), updated_by = 'sync-api' WHERE id = 89 AND fase_id = 2;");
            migrationBuilder.Sql("UPDATE partidos SET partido_id_api = '679c9c8a5749c4077500e084', updated_at = now(), updated_by = 'sync-api' WHERE id = 90 AND fase_id = 2;");
            migrationBuilder.Sql("UPDATE partidos SET partido_id_api = '679c9c8a5749c4077500e085', updated_at = now(), updated_by = 'sync-api' WHERE id = 91 AND fase_id = 2;");
            migrationBuilder.Sql("UPDATE partidos SET partido_id_api = '679c9c8a5749c4077500e086', updated_at = now(), updated_by = 'sync-api' WHERE id = 92 AND fase_id = 2;");
            migrationBuilder.Sql("UPDATE partidos SET partido_id_api = '679c9c8a5749c4077500e087', updated_at = now(), updated_by = 'sync-api' WHERE id = 93 AND fase_id = 2;");
            migrationBuilder.Sql("UPDATE partidos SET partido_id_api = '679c9c8a5749c4077500e088', updated_at = now(), updated_by = 'sync-api' WHERE id = 94 AND fase_id = 2;");
            migrationBuilder.Sql("UPDATE partidos SET partido_id_api = '679c9c8a5749c4077500e089', updated_at = now(), updated_by = 'sync-api' WHERE id = 95 AND fase_id = 2;");
            migrationBuilder.Sql("UPDATE partidos SET partido_id_api = '679c9c8a5749c4077500e08a', updated_at = now(), updated_by = 'sync-api' WHERE id = 96 AND fase_id = 2;");
            migrationBuilder.Sql("UPDATE partidos SET partido_id_api = '679c9c8a5749c4077500e08b', updated_at = now(), updated_by = 'sync-api' WHERE id = 97 AND fase_id = 2;");
            migrationBuilder.Sql("UPDATE partidos SET partido_id_api = '679c9c8a5749c4077500e08c', updated_at = now(), updated_by = 'sync-api' WHERE id = 98 AND fase_id = 2;");
            migrationBuilder.Sql("UPDATE partidos SET partido_id_api = '679c9c8a5749c4077500e08d', updated_at = now(), updated_by = 'sync-api' WHERE id = 99 AND fase_id = 2;");
            migrationBuilder.Sql("UPDATE partidos SET partido_id_api = '679c9c8a5749c4077500e08e', updated_at = now(), updated_by = 'sync-api' WHERE id = 100 AND fase_id = 2;");
            migrationBuilder.Sql("UPDATE partidos SET partido_id_api = '679c9c8a5749c4077500e08f', updated_at = now(), updated_by = 'sync-api' WHERE id = 101 AND fase_id = 2;");
            migrationBuilder.Sql("UPDATE partidos SET partido_id_api = '679c9c8a5749c4077500e090', updated_at = now(), updated_by = 'sync-api' WHERE id = 102 AND fase_id = 2;");
            migrationBuilder.Sql("UPDATE partidos SET partido_id_api = '679c9c8a5749c4077500e091', updated_at = now(), updated_by = 'sync-api' WHERE id = 103 AND fase_id = 2;");
            migrationBuilder.Sql("UPDATE partidos SET partido_id_api = '679c9c8a5749c4077500e092', updated_at = now(), updated_by = 'sync-api' WHERE id = 104 AND fase_id = 2;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE partidos SET partido_id_api = NULL WHERE fase_id = 2;");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quinela.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCamposPartidoEliminatoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "aplica_definicion_penales",
                table: "partidos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "equipo_ganador_id",
                table: "partidos",
                type: "integer",
                nullable: true);

            // defaultValue: 1 (Grupos) para que los partidos existentes queden en esa
            // fase antes de activar la FK (no existe fase con id 0).
            migrationBuilder.AddColumn<int>(
                name: "fase_id",
                table: "partidos",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "partido_ganador_local_id",
                table: "partidos",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "partido_ganador_visitante_id",
                table: "partidos",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "partido_se_definira_en_penales",
                table: "partidos",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "penales_anotados_local",
                table: "partidos",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "penales_anotados_visitante",
                table: "partidos",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 10,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 11,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 12,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 13,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 14,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 15,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 16,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 17,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 18,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 19,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 20,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 21,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 22,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 23,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 24,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 25,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 26,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 27,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 28,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 29,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 30,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 31,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 32,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 33,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 34,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 35,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 36,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 37,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 38,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 39,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 40,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 41,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 42,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 43,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 44,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 45,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 46,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 47,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 48,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 49,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 50,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 51,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 52,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 53,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 54,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 55,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 56,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 57,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 58,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 59,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 60,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 61,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 62,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 63,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 64,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 65,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 66,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 67,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 68,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 69,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 70,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 71,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 72,
                columns: new[] { "equipo_ganador_id", "fase_id", "partido_ganador_local_id", "partido_ganador_visitante_id", "partido_se_definira_en_penales", "penales_anotados_local", "penales_anotados_visitante" },
                values: new object[] { null, 1, null, null, null, null, null });

            migrationBuilder.CreateIndex(
                name: "IX_partidos_equipo_ganador_id",
                table: "partidos",
                column: "equipo_ganador_id");

            migrationBuilder.CreateIndex(
                name: "IX_partidos_fase_id",
                table: "partidos",
                column: "fase_id");

            migrationBuilder.CreateIndex(
                name: "IX_partidos_partido_ganador_local_id",
                table: "partidos",
                column: "partido_ganador_local_id");

            migrationBuilder.CreateIndex(
                name: "IX_partidos_partido_ganador_visitante_id",
                table: "partidos",
                column: "partido_ganador_visitante_id");

            migrationBuilder.AddForeignKey(
                name: "FK_partidos_equipos_equipo_ganador_id",
                table: "partidos",
                column: "equipo_ganador_id",
                principalTable: "equipos",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_partidos_fases_fase_id",
                table: "partidos",
                column: "fase_id",
                principalTable: "fases",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_partidos_partidos_partido_ganador_local_id",
                table: "partidos",
                column: "partido_ganador_local_id",
                principalTable: "partidos",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_partidos_partidos_partido_ganador_visitante_id",
                table: "partidos",
                column: "partido_ganador_visitante_id",
                principalTable: "partidos",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_partidos_equipos_equipo_ganador_id",
                table: "partidos");

            migrationBuilder.DropForeignKey(
                name: "FK_partidos_fases_fase_id",
                table: "partidos");

            migrationBuilder.DropForeignKey(
                name: "FK_partidos_partidos_partido_ganador_local_id",
                table: "partidos");

            migrationBuilder.DropForeignKey(
                name: "FK_partidos_partidos_partido_ganador_visitante_id",
                table: "partidos");

            migrationBuilder.DropIndex(
                name: "IX_partidos_equipo_ganador_id",
                table: "partidos");

            migrationBuilder.DropIndex(
                name: "IX_partidos_fase_id",
                table: "partidos");

            migrationBuilder.DropIndex(
                name: "IX_partidos_partido_ganador_local_id",
                table: "partidos");

            migrationBuilder.DropIndex(
                name: "IX_partidos_partido_ganador_visitante_id",
                table: "partidos");

            migrationBuilder.DropColumn(
                name: "aplica_definicion_penales",
                table: "partidos");

            migrationBuilder.DropColumn(
                name: "equipo_ganador_id",
                table: "partidos");

            migrationBuilder.DropColumn(
                name: "fase_id",
                table: "partidos");

            migrationBuilder.DropColumn(
                name: "partido_ganador_local_id",
                table: "partidos");

            migrationBuilder.DropColumn(
                name: "partido_ganador_visitante_id",
                table: "partidos");

            migrationBuilder.DropColumn(
                name: "partido_se_definira_en_penales",
                table: "partidos");

            migrationBuilder.DropColumn(
                name: "penales_anotados_local",
                table: "partidos");

            migrationBuilder.DropColumn(
                name: "penales_anotados_visitante",
                table: "partidos");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Quinela.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPartido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "partidos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fecha_partido = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    grupo_id = table.Column<int>(type: "integer", nullable: false),
                    equipo_local_id = table.Column<int>(type: "integer", nullable: false),
                    equipo_visitante_id = table.Column<int>(type: "integer", nullable: false),
                    resultado_local_id = table.Column<int>(type: "integer", nullable: true),
                    resultado_visitante_id = table.Column<int>(type: "integer", nullable: true),
                    pts_local = table.Column<int>(type: "integer", nullable: true),
                    pts_visitante = table.Column<int>(type: "integer", nullable: true),
                    tipo_partido_id = table.Column<int>(type: "integer", nullable: false),
                    estado = table.Column<char>(type: "char(1)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_partidos", x => x.id);
                    table.ForeignKey(
                        name: "FK_partidos_equipos_equipo_local_id",
                        column: x => x.equipo_local_id,
                        principalTable: "equipos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_partidos_equipos_equipo_visitante_id",
                        column: x => x.equipo_visitante_id,
                        principalTable: "equipos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_partidos_grupos_grupo_id",
                        column: x => x.grupo_id,
                        principalTable: "grupos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_partidos_tipos_partido_tipo_partido_id",
                        column: x => x.tipo_partido_id,
                        principalTable: "tipos_partido",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "tipos_partido",
                columns: new[] { "id", "active", "created_at", "created_by", "descripcion", "pts_partido_empate", "pts_partido_victoria", "pts_quinela_resultado_acertado", "pts_quinela_resultado_exacto", "updated_at", "updated_by" },
                values: new object[] { 1, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Fase de grupos", 0, 0, 0, 0, null, null });

            migrationBuilder.InsertData(
                table: "partidos",
                columns: new[] { "id", "active", "created_at", "created_by", "equipo_local_id", "equipo_visitante_id", "estado", "fecha_partido", "grupo_id", "pts_local", "pts_visitante", "resultado_local_id", "resultado_visitante_id", "tipo_partido_id", "updated_at", "updated_by" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 1, 2, 'P', new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null, null, null, 1, null, null },
                    { 2, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 3, 4, 'P', new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null, null, null, 1, null, null },
                    { 3, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 5, 6, 'P', new DateTime(2026, 6, 12, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null, null, null, 1, null, null },
                    { 4, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 13, 14, 'P', new DateTime(2026, 6, 12, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null, null, null, 1, null, null },
                    { 5, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 7, 8, 'P', new DateTime(2026, 6, 13, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null, null, null, 1, null, null },
                    { 6, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 9, 10, 'P', new DateTime(2026, 6, 13, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null, null, null, 1, null, null },
                    { 7, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 11, 12, 'P', new DateTime(2026, 6, 13, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null, null, null, 1, null, null },
                    { 8, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 15, 16, 'P', new DateTime(2026, 6, 13, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null, null, null, 1, null, null },
                    { 9, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 17, 18, 'P', new DateTime(2026, 6, 14, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null, null, null, 1, null, null },
                    { 10, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 21, 22, 'P', new DateTime(2026, 6, 14, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, null, null, null, 1, null, null },
                    { 11, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 19, 20, 'P', new DateTime(2026, 6, 14, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null, null, null, 1, null, null },
                    { 12, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 23, 24, 'P', new DateTime(2026, 6, 14, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, null, null, null, 1, null, null },
                    { 13, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 29, 30, 'P', new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), 8, null, null, null, null, 1, null, null },
                    { 14, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 25, 26, 'P', new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), 7, null, null, null, null, 1, null, null },
                    { 15, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 31, 32, 'P', new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), 8, null, null, null, null, 1, null, null },
                    { 16, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 27, 28, 'P', new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), 7, null, null, null, null, 1, null, null },
                    { 17, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 33, 34, 'P', new DateTime(2026, 6, 16, 0, 0, 0, 0, DateTimeKind.Utc), 9, null, null, null, null, 1, null, null },
                    { 18, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 35, 36, 'P', new DateTime(2026, 6, 16, 0, 0, 0, 0, DateTimeKind.Utc), 9, null, null, null, null, 1, null, null },
                    { 19, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 37, 38, 'P', new DateTime(2026, 6, 16, 0, 0, 0, 0, DateTimeKind.Utc), 10, null, null, null, null, 1, null, null },
                    { 20, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 39, 40, 'P', new DateTime(2026, 6, 16, 0, 0, 0, 0, DateTimeKind.Utc), 10, null, null, null, null, 1, null, null },
                    { 21, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 41, 42, 'P', new DateTime(2026, 6, 17, 0, 0, 0, 0, DateTimeKind.Utc), 11, null, null, null, null, 1, null, null },
                    { 22, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 45, 46, 'P', new DateTime(2026, 6, 17, 0, 0, 0, 0, DateTimeKind.Utc), 12, null, null, null, null, 1, null, null },
                    { 23, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 47, 48, 'P', new DateTime(2026, 6, 17, 0, 0, 0, 0, DateTimeKind.Utc), 12, null, null, null, null, 1, null, null },
                    { 24, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 43, 44, 'P', new DateTime(2026, 6, 17, 0, 0, 0, 0, DateTimeKind.Utc), 11, null, null, null, null, 1, null, null },
                    { 25, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 4, 2, 'P', new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null, null, null, 1, null, null },
                    { 26, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 8, 6, 'P', new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null, null, null, 1, null, null },
                    { 27, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 5, 7, 'P', new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null, null, null, 1, null, null },
                    { 28, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 1, 3, 'P', new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null, null, null, 1, null, null },
                    { 29, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 13, 15, 'P', new DateTime(2026, 6, 19, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null, null, null, 1, null, null },
                    { 30, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 12, 10, 'P', new DateTime(2026, 6, 19, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null, null, null, 1, null, null },
                    { 31, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 9, 11, 'P', new DateTime(2026, 6, 19, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null, null, null, 1, null, null },
                    { 32, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 16, 14, 'P', new DateTime(2026, 6, 19, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null, null, null, 1, null, null },
                    { 33, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 21, 23, 'P', new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, null, null, null, 1, null, null },
                    { 34, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 17, 19, 'P', new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null, null, null, 1, null, null },
                    { 35, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 20, 18, 'P', new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null, null, null, 1, null, null },
                    { 36, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 24, 22, 'P', new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, null, null, null, 1, null, null },
                    { 37, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 29, 31, 'P', new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), 8, null, null, null, null, 1, null, null },
                    { 38, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 25, 27, 'P', new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), 7, null, null, null, null, 1, null, null },
                    { 39, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 32, 30, 'P', new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), 8, null, null, null, null, 1, null, null },
                    { 40, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 28, 26, 'P', new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), 7, null, null, null, null, 1, null, null },
                    { 41, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 37, 39, 'P', new DateTime(2026, 6, 22, 0, 0, 0, 0, DateTimeKind.Utc), 10, null, null, null, null, 1, null, null },
                    { 42, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 33, 35, 'P', new DateTime(2026, 6, 22, 0, 0, 0, 0, DateTimeKind.Utc), 9, null, null, null, null, 1, null, null },
                    { 43, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 36, 34, 'P', new DateTime(2026, 6, 22, 0, 0, 0, 0, DateTimeKind.Utc), 9, null, null, null, null, 1, null, null },
                    { 44, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 40, 38, 'P', new DateTime(2026, 6, 22, 0, 0, 0, 0, DateTimeKind.Utc), 10, null, null, null, null, 1, null, null },
                    { 45, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 41, 43, 'P', new DateTime(2026, 6, 23, 0, 0, 0, 0, DateTimeKind.Utc), 11, null, null, null, null, 1, null, null },
                    { 46, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 45, 47, 'P', new DateTime(2026, 6, 23, 0, 0, 0, 0, DateTimeKind.Utc), 12, null, null, null, null, 1, null, null },
                    { 47, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 48, 46, 'P', new DateTime(2026, 6, 23, 0, 0, 0, 0, DateTimeKind.Utc), 12, null, null, null, null, 1, null, null },
                    { 48, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 44, 42, 'P', new DateTime(2026, 6, 23, 0, 0, 0, 0, DateTimeKind.Utc), 11, null, null, null, null, 1, null, null },
                    { 49, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 8, 5, 'P', new DateTime(2026, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null, null, null, 1, null, null },
                    { 50, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 6, 7, 'P', new DateTime(2026, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, null, null, null, 1, null, null },
                    { 51, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 10, 11, 'P', new DateTime(2026, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null, null, null, 1, null, null },
                    { 52, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 12, 9, 'P', new DateTime(2026, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), 3, null, null, null, null, 1, null, null },
                    { 53, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 2, 3, 'P', new DateTime(2026, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null, null, null, 1, null, null },
                    { 54, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 4, 1, 'P', new DateTime(2026, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null, null, null, 1, null, null },
                    { 55, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 18, 19, 'P', new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null, null, null, 1, null, null },
                    { 56, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 20, 17, 'P', new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Utc), 5, null, null, null, null, 1, null, null },
                    { 57, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 24, 21, 'P', new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, null, null, null, 1, null, null },
                    { 58, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 22, 23, 'P', new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Utc), 6, null, null, null, null, 1, null, null },
                    { 59, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 16, 13, 'P', new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null, null, null, 1, null, null },
                    { 60, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 14, 15, 'P', new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Utc), 4, null, null, null, null, 1, null, null },
                    { 61, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 36, 33, 'P', new DateTime(2026, 6, 26, 0, 0, 0, 0, DateTimeKind.Utc), 9, null, null, null, null, 1, null, null },
                    { 62, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 34, 35, 'P', new DateTime(2026, 6, 26, 0, 0, 0, 0, DateTimeKind.Utc), 9, null, null, null, null, 1, null, null },
                    { 63, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 30, 31, 'P', new DateTime(2026, 6, 26, 0, 0, 0, 0, DateTimeKind.Utc), 8, null, null, null, null, 1, null, null },
                    { 64, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 32, 29, 'P', new DateTime(2026, 6, 26, 0, 0, 0, 0, DateTimeKind.Utc), 8, null, null, null, null, 1, null, null },
                    { 65, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 28, 25, 'P', new DateTime(2026, 6, 26, 0, 0, 0, 0, DateTimeKind.Utc), 7, null, null, null, null, 1, null, null },
                    { 66, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 26, 27, 'P', new DateTime(2026, 6, 26, 0, 0, 0, 0, DateTimeKind.Utc), 7, null, null, null, null, 1, null, null },
                    { 67, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 48, 45, 'P', new DateTime(2026, 6, 27, 0, 0, 0, 0, DateTimeKind.Utc), 12, null, null, null, null, 1, null, null },
                    { 68, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 46, 47, 'P', new DateTime(2026, 6, 27, 0, 0, 0, 0, DateTimeKind.Utc), 12, null, null, null, null, 1, null, null },
                    { 69, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 44, 41, 'P', new DateTime(2026, 6, 27, 0, 0, 0, 0, DateTimeKind.Utc), 11, null, null, null, null, 1, null, null },
                    { 70, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 42, 43, 'P', new DateTime(2026, 6, 27, 0, 0, 0, 0, DateTimeKind.Utc), 11, null, null, null, null, 1, null, null },
                    { 71, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 38, 39, 'P', new DateTime(2026, 6, 27, 0, 0, 0, 0, DateTimeKind.Utc), 10, null, null, null, null, 1, null, null },
                    { 72, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 40, 37, 'P', new DateTime(2026, 6, 27, 0, 0, 0, 0, DateTimeKind.Utc), 10, null, null, null, null, 1, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_partidos_equipo_local_id",
                table: "partidos",
                column: "equipo_local_id");

            migrationBuilder.CreateIndex(
                name: "IX_partidos_equipo_visitante_id",
                table: "partidos",
                column: "equipo_visitante_id");

            migrationBuilder.CreateIndex(
                name: "IX_partidos_grupo_id",
                table: "partidos",
                column: "grupo_id");

            migrationBuilder.CreateIndex(
                name: "IX_partidos_tipo_partido_id",
                table: "partidos",
                column: "tipo_partido_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "partidos");

            migrationBuilder.DeleteData(
                table: "tipos_partido",
                keyColumn: "id",
                keyValue: 1);
        }
    }
}

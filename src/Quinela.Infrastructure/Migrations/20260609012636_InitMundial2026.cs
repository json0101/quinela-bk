using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Quinela.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitMundial2026 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "equipos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    confederacion = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    anfitrion = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_equipos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "grupos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grupos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "grupos_equipos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    grupo_id = table.Column<int>(type: "integer", nullable: false),
                    equipo_id = table.Column<int>(type: "integer", nullable: false),
                    pts = table.Column<int>(type: "integer", nullable: false),
                    gf = table.Column<int>(type: "integer", nullable: false),
                    gc = table.Column<int>(type: "integer", nullable: false),
                    diff = table.Column<int>(type: "integer", nullable: false),
                    posicion = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grupos_equipos", x => x.id);
                    table.ForeignKey(
                        name: "FK_grupos_equipos_equipos_equipo_id",
                        column: x => x.equipo_id,
                        principalTable: "equipos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_grupos_equipos_grupos_grupo_id",
                        column: x => x.grupo_id,
                        principalTable: "grupos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "equipos",
                columns: new[] { "id", "active", "anfitrion", "confederacion", "created_at", "created_by", "nombre", "updated_at", "updated_by" },
                values: new object[,]
                {
                    { 1, true, true, "CONCACAF", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "México", null, null },
                    { 2, true, false, "CAF", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Sudáfrica", null, null },
                    { 3, true, false, "AFC", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Corea del Sur", null, null },
                    { 4, true, false, "UEFA", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "República Checa", null, null },
                    { 5, true, true, "CONCACAF", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Canadá", null, null },
                    { 6, true, false, "UEFA", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Bosnia y Herzegovina", null, null },
                    { 7, true, false, "AFC", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Qatar", null, null },
                    { 8, true, false, "UEFA", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Suiza", null, null },
                    { 9, true, false, "CONMEBOL", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Brasil", null, null },
                    { 10, true, false, "CAF", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Marruecos", null, null },
                    { 11, true, false, "CONCACAF", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Haití", null, null },
                    { 12, true, false, "UEFA", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Escocia", null, null },
                    { 13, true, true, "CONCACAF", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Estados Unidos", null, null },
                    { 14, true, false, "CONMEBOL", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Paraguay", null, null },
                    { 15, true, false, "AFC", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Australia", null, null },
                    { 16, true, false, "UEFA", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Turquía", null, null },
                    { 17, true, false, "UEFA", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Alemania", null, null },
                    { 18, true, false, "CONCACAF", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Curazao", null, null },
                    { 19, true, false, "CAF", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Costa de Marfil", null, null },
                    { 20, true, false, "CONMEBOL", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Ecuador", null, null },
                    { 21, true, false, "UEFA", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Países Bajos", null, null },
                    { 22, true, false, "AFC", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Japón", null, null },
                    { 23, true, false, "UEFA", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Suecia", null, null },
                    { 24, true, false, "CAF", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Túnez", null, null },
                    { 25, true, false, "UEFA", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Bélgica", null, null },
                    { 26, true, false, "CAF", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Egipto", null, null },
                    { 27, true, false, "AFC", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Irán", null, null },
                    { 28, true, false, "OFC", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Nueva Zelanda", null, null },
                    { 29, true, false, "UEFA", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "España", null, null },
                    { 30, true, false, "CAF", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Cabo Verde", null, null },
                    { 31, true, false, "AFC", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Arabia Saudita", null, null },
                    { 32, true, false, "CONMEBOL", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Uruguay", null, null },
                    { 33, true, false, "UEFA", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Francia", null, null },
                    { 34, true, false, "CAF", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Senegal", null, null },
                    { 35, true, false, "AFC", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Irak", null, null },
                    { 36, true, false, "UEFA", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Noruega", null, null },
                    { 37, true, false, "CONMEBOL", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Argentina", null, null },
                    { 38, true, false, "CAF", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Argelia", null, null },
                    { 39, true, false, "UEFA", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Austria", null, null },
                    { 40, true, false, "AFC", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Jordania", null, null },
                    { 41, true, false, "UEFA", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Portugal", null, null },
                    { 42, true, false, "CAF", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "RD Congo", null, null },
                    { 43, true, false, "AFC", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Uzbekistán", null, null },
                    { 44, true, false, "CONMEBOL", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Colombia", null, null },
                    { 45, true, false, "UEFA", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Inglaterra", null, null },
                    { 46, true, false, "UEFA", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Croacia", null, null },
                    { 47, true, false, "CAF", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Ghana", null, null },
                    { 48, true, false, "CONCACAF", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Panamá", null, null }
                });

            migrationBuilder.InsertData(
                table: "grupos",
                columns: new[] { "id", "active", "created_at", "created_by", "nombre", "updated_at", "updated_by" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "A", null, null },
                    { 2, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "B", null, null },
                    { 3, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "C", null, null },
                    { 4, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "D", null, null },
                    { 5, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "E", null, null },
                    { 6, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "F", null, null },
                    { 7, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "G", null, null },
                    { 8, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "H", null, null },
                    { 9, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "I", null, null },
                    { 10, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "J", null, null },
                    { 11, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "K", null, null },
                    { 12, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "L", null, null }
                });

            migrationBuilder.InsertData(
                table: "grupos_equipos",
                columns: new[] { "id", "active", "created_at", "created_by", "diff", "equipo_id", "gc", "gf", "grupo_id", "posicion", "pts", "updated_at", "updated_by" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 1, 0, 0, 1, 1, 0, null, null },
                    { 2, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 2, 0, 0, 1, 2, 0, null, null },
                    { 3, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 3, 0, 0, 1, 3, 0, null, null },
                    { 4, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 4, 0, 0, 1, 4, 0, null, null },
                    { 5, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 5, 0, 0, 2, 1, 0, null, null },
                    { 6, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 6, 0, 0, 2, 2, 0, null, null },
                    { 7, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 7, 0, 0, 2, 3, 0, null, null },
                    { 8, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 8, 0, 0, 2, 4, 0, null, null },
                    { 9, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 9, 0, 0, 3, 1, 0, null, null },
                    { 10, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 10, 0, 0, 3, 2, 0, null, null },
                    { 11, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 11, 0, 0, 3, 3, 0, null, null },
                    { 12, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 12, 0, 0, 3, 4, 0, null, null },
                    { 13, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 13, 0, 0, 4, 1, 0, null, null },
                    { 14, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 14, 0, 0, 4, 2, 0, null, null },
                    { 15, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 15, 0, 0, 4, 3, 0, null, null },
                    { 16, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 16, 0, 0, 4, 4, 0, null, null },
                    { 17, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 17, 0, 0, 5, 1, 0, null, null },
                    { 18, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 18, 0, 0, 5, 2, 0, null, null },
                    { 19, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 19, 0, 0, 5, 3, 0, null, null },
                    { 20, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 20, 0, 0, 5, 4, 0, null, null },
                    { 21, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 21, 0, 0, 6, 1, 0, null, null },
                    { 22, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 22, 0, 0, 6, 2, 0, null, null },
                    { 23, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 23, 0, 0, 6, 3, 0, null, null },
                    { 24, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 24, 0, 0, 6, 4, 0, null, null },
                    { 25, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 25, 0, 0, 7, 1, 0, null, null },
                    { 26, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 26, 0, 0, 7, 2, 0, null, null },
                    { 27, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 27, 0, 0, 7, 3, 0, null, null },
                    { 28, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 28, 0, 0, 7, 4, 0, null, null },
                    { 29, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 29, 0, 0, 8, 1, 0, null, null },
                    { 30, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 30, 0, 0, 8, 2, 0, null, null },
                    { 31, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 31, 0, 0, 8, 3, 0, null, null },
                    { 32, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 32, 0, 0, 8, 4, 0, null, null },
                    { 33, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 33, 0, 0, 9, 1, 0, null, null },
                    { 34, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 34, 0, 0, 9, 2, 0, null, null },
                    { 35, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 35, 0, 0, 9, 3, 0, null, null },
                    { 36, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 36, 0, 0, 9, 4, 0, null, null },
                    { 37, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 37, 0, 0, 10, 1, 0, null, null },
                    { 38, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 38, 0, 0, 10, 2, 0, null, null },
                    { 39, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 39, 0, 0, 10, 3, 0, null, null },
                    { 40, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 40, 0, 0, 10, 4, 0, null, null },
                    { 41, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 41, 0, 0, 11, 1, 0, null, null },
                    { 42, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 42, 0, 0, 11, 2, 0, null, null },
                    { 43, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 43, 0, 0, 11, 3, 0, null, null },
                    { 44, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 44, 0, 0, 11, 4, 0, null, null },
                    { 45, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 45, 0, 0, 12, 1, 0, null, null },
                    { 46, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 46, 0, 0, 12, 2, 0, null, null },
                    { 47, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 47, 0, 0, 12, 3, 0, null, null },
                    { 48, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 0, 48, 0, 0, 12, 4, 0, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_equipos_nombre",
                table: "equipos",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_grupos_nombre",
                table: "grupos",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_grupos_equipos_equipo_id",
                table: "grupos_equipos",
                column: "equipo_id");

            migrationBuilder.CreateIndex(
                name: "IX_grupos_equipos_grupo_id_equipo_id",
                table: "grupos_equipos",
                columns: new[] { "grupo_id", "equipo_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "grupos_equipos");

            migrationBuilder.DropTable(
                name: "equipos");

            migrationBuilder.DropTable(
                name: "grupos");
        }
    }
}

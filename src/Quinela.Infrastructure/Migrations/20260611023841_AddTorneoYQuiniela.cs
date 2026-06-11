using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Quinela.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTorneoYQuiniela : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_predicciones_partido_id_username",
                table: "predicciones");

            migrationBuilder.DropIndex(
                name: "IX_grupos_nombre",
                table: "grupos");

            migrationBuilder.DropIndex(
                name: "IX_equipos_nombre",
                table: "equipos");

            migrationBuilder.AddColumn<int>(
                name: "quiniela_id",
                table: "ranking",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "quiniela_id",
                table: "predicciones",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "torneo_id",
                table: "partidos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "torneo_id",
                table: "grupos_equipos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "torneo_id",
                table: "grupos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "torneo_id",
                table: "equipos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "torneos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    descripcion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_torneos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "quinielas",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    reglas = table.Column<string>(type: "text", nullable: false),
                    torneo_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quinielas", x => x.id);
                    table.ForeignKey(
                        name: "FK_quinielas_torneos_torneo_id",
                        column: x => x.torneo_id,
                        principalTable: "torneos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 1,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 2,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 3,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 4,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 5,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 6,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 7,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 8,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 9,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 10,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 11,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 12,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 13,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 14,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 15,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 16,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 17,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 18,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 19,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 20,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 21,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 22,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 23,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 24,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 25,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 26,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 27,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 28,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 29,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 30,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 31,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 32,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 33,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 34,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 35,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 36,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 37,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 38,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 39,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 40,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 41,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 42,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 43,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 44,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 45,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 46,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 47,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 48,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos",
                keyColumn: "id",
                keyValue: 1,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos",
                keyColumn: "id",
                keyValue: 2,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos",
                keyColumn: "id",
                keyValue: 3,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos",
                keyColumn: "id",
                keyValue: 4,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos",
                keyColumn: "id",
                keyValue: 5,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos",
                keyColumn: "id",
                keyValue: 6,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos",
                keyColumn: "id",
                keyValue: 7,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos",
                keyColumn: "id",
                keyValue: 8,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos",
                keyColumn: "id",
                keyValue: 9,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos",
                keyColumn: "id",
                keyValue: 10,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos",
                keyColumn: "id",
                keyValue: 11,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos",
                keyColumn: "id",
                keyValue: 12,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 1,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 2,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 3,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 4,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 5,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 6,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 7,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 8,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 9,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 10,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 11,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 12,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 13,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 14,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 15,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 16,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 17,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 18,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 19,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 20,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 21,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 22,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 23,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 24,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 25,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 26,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 27,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 28,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 29,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 30,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 31,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 32,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 33,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 34,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 35,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 36,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 37,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 38,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 39,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 40,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 41,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 42,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 43,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 44,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 45,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 46,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 47,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "grupos_equipos",
                keyColumn: "id",
                keyValue: 48,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 1,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 2,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 3,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 4,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 5,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 6,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 7,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 8,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 9,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 10,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 11,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 12,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 13,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 14,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 15,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 16,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 17,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 18,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 19,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 20,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 21,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 22,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 23,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 24,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 25,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 26,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 27,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 28,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 29,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 30,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 31,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 32,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 33,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 34,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 35,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 36,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 37,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 38,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 39,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 40,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 41,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 42,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 43,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 44,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 45,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 46,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 47,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 48,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 49,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 50,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 51,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 52,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 53,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 54,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 55,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 56,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 57,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 58,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 59,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 60,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 61,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 62,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 63,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 64,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 65,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 66,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 67,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 68,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 69,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 70,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 71,
                column: "torneo_id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "partidos",
                keyColumn: "id",
                keyValue: 72,
                column: "torneo_id",
                value: 1);

            migrationBuilder.InsertData(
                table: "torneos",
                columns: new[] { "id", "active", "created_at", "created_by", "descripcion", "updated_at", "updated_by" },
                values: new object[] { 1, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Copa Mundial de la FIFA 2026", null, null });

            migrationBuilder.InsertData(
                table: "quinielas",
                columns: new[] { "id", "active", "created_at", "created_by", "nombre", "reglas", "torneo_id", "updated_at", "updated_by" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Quiniela Cattrachas", "Reglas por definir.", 1, null, null },
                    { 2, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Quiniela Tegra", "Reglas por definir.", 1, null, null },
                    { 3, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Quiniela Impex", "Reglas por definir.", 1, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ranking_quiniela_id_usuario",
                table: "ranking",
                columns: new[] { "quiniela_id", "usuario" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_predicciones_partido_id",
                table: "predicciones",
                column: "partido_id");

            migrationBuilder.CreateIndex(
                name: "IX_predicciones_quiniela_id_partido_id_username",
                table: "predicciones",
                columns: new[] { "quiniela_id", "partido_id", "username" },
                unique: true,
                filter: "active = true");

            migrationBuilder.CreateIndex(
                name: "IX_partidos_torneo_id",
                table: "partidos",
                column: "torneo_id");

            migrationBuilder.CreateIndex(
                name: "IX_grupos_equipos_torneo_id",
                table: "grupos_equipos",
                column: "torneo_id");

            migrationBuilder.CreateIndex(
                name: "IX_grupos_torneo_id_nombre",
                table: "grupos",
                columns: new[] { "torneo_id", "nombre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_equipos_torneo_id_nombre",
                table: "equipos",
                columns: new[] { "torneo_id", "nombre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_quinielas_torneo_id",
                table: "quinielas",
                column: "torneo_id");

            migrationBuilder.AddForeignKey(
                name: "FK_equipos_torneos_torneo_id",
                table: "equipos",
                column: "torneo_id",
                principalTable: "torneos",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_grupos_torneos_torneo_id",
                table: "grupos",
                column: "torneo_id",
                principalTable: "torneos",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_grupos_equipos_torneos_torneo_id",
                table: "grupos_equipos",
                column: "torneo_id",
                principalTable: "torneos",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_partidos_torneos_torneo_id",
                table: "partidos",
                column: "torneo_id",
                principalTable: "torneos",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_predicciones_quinielas_quiniela_id",
                table: "predicciones",
                column: "quiniela_id",
                principalTable: "quinielas",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ranking_quinielas_quiniela_id",
                table: "ranking",
                column: "quiniela_id",
                principalTable: "quinielas",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_equipos_torneos_torneo_id",
                table: "equipos");

            migrationBuilder.DropForeignKey(
                name: "FK_grupos_torneos_torneo_id",
                table: "grupos");

            migrationBuilder.DropForeignKey(
                name: "FK_grupos_equipos_torneos_torneo_id",
                table: "grupos_equipos");

            migrationBuilder.DropForeignKey(
                name: "FK_partidos_torneos_torneo_id",
                table: "partidos");

            migrationBuilder.DropForeignKey(
                name: "FK_predicciones_quinielas_quiniela_id",
                table: "predicciones");

            migrationBuilder.DropForeignKey(
                name: "FK_ranking_quinielas_quiniela_id",
                table: "ranking");

            migrationBuilder.DropTable(
                name: "quinielas");

            migrationBuilder.DropTable(
                name: "torneos");

            migrationBuilder.DropIndex(
                name: "IX_ranking_quiniela_id_usuario",
                table: "ranking");

            migrationBuilder.DropIndex(
                name: "IX_predicciones_partido_id",
                table: "predicciones");

            migrationBuilder.DropIndex(
                name: "IX_predicciones_quiniela_id_partido_id_username",
                table: "predicciones");

            migrationBuilder.DropIndex(
                name: "IX_partidos_torneo_id",
                table: "partidos");

            migrationBuilder.DropIndex(
                name: "IX_grupos_equipos_torneo_id",
                table: "grupos_equipos");

            migrationBuilder.DropIndex(
                name: "IX_grupos_torneo_id_nombre",
                table: "grupos");

            migrationBuilder.DropIndex(
                name: "IX_equipos_torneo_id_nombre",
                table: "equipos");

            migrationBuilder.DropColumn(
                name: "quiniela_id",
                table: "ranking");

            migrationBuilder.DropColumn(
                name: "quiniela_id",
                table: "predicciones");

            migrationBuilder.DropColumn(
                name: "torneo_id",
                table: "partidos");

            migrationBuilder.DropColumn(
                name: "torneo_id",
                table: "grupos_equipos");

            migrationBuilder.DropColumn(
                name: "torneo_id",
                table: "grupos");

            migrationBuilder.DropColumn(
                name: "torneo_id",
                table: "equipos");

            migrationBuilder.CreateIndex(
                name: "IX_predicciones_partido_id_username",
                table: "predicciones",
                columns: new[] { "partido_id", "username" },
                unique: true,
                filter: "active = true");

            migrationBuilder.CreateIndex(
                name: "IX_grupos_nombre",
                table: "grupos",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_equipos_nombre",
                table: "equipos",
                column: "nombre",
                unique: true);
        }
    }
}

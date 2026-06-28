using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Quinela.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1) Crear la tabla nueva (fases) con su FK a torneos.
            migrationBuilder.CreateTable(
                name: "fases",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    descripcion = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    torneo_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fases", x => x.id);
                    table.ForeignKey(
                        name: "FK_fases_torneos_torneo_id",
                        column: x => x.torneo_id,
                        principalTable: "torneos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_fases_torneo_id_descripcion",
                table: "fases",
                columns: new[] { "torneo_id", "descripcion" },
                unique: true);

            // 2) Ingresar la data de fases (Grupos / Eliminatoria).
            migrationBuilder.InsertData(
                table: "fases",
                columns: new[] { "id", "active", "created_at", "created_by", "descripcion", "torneo_id", "updated_at", "updated_by" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Grupos", 1, null, null },
                    { 2, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Eliminatoria", 1, null, null }
                });

            // 3) Agregar el campo nuevo a tipos_partido. defaultValue: 1 (Grupos)
            //    para que la data existente quede asignada a esa fase antes de activar la FK.
            migrationBuilder.AddColumn<int>(
                name: "fase_id",
                table: "tipos_partido",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.UpdateData(
                table: "tipos_partido",
                keyColumn: "id",
                keyValue: 1,
                column: "fase_id",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_tipos_partido_fase_id",
                table: "tipos_partido",
                column: "fase_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tipos_partido_fases_fase_id",
                table: "tipos_partido",
                column: "fase_id",
                principalTable: "fases",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Inverso: primero eliminar el campo nuevo junto con su relación e índice...
            migrationBuilder.DropForeignKey(
                name: "FK_tipos_partido_fases_fase_id",
                table: "tipos_partido");

            migrationBuilder.DropIndex(
                name: "IX_tipos_partido_fase_id",
                table: "tipos_partido");

            migrationBuilder.DropColumn(
                name: "fase_id",
                table: "tipos_partido");

            // ...y luego la tabla nueva.
            migrationBuilder.DropTable(
                name: "fases");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Quinela.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTipoPartido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tipos_partido",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    descripcion = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    pts_partido_victoria = table.Column<int>(type: "integer", nullable: false),
                    pts_partido_empate = table.Column<int>(type: "integer", nullable: false),
                    pts_quinela_resultado_exacto = table.Column<int>(type: "integer", nullable: false),
                    pts_quinela_resultado_acertado = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipos_partido", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tipos_partido_descripcion",
                table: "tipos_partido",
                column: "descripcion",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tipos_partido");
        }
    }
}

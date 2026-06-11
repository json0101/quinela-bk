using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Quinela.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUsuarioQuiniela : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "usuarios_quinielas",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    quiniela_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios_quinielas", x => x.id);
                    table.ForeignKey(
                        name: "FK_usuarios_quinielas_quinielas_quiniela_id",
                        column: x => x.quiniela_id,
                        principalTable: "quinielas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "usuarios_quinielas",
                columns: new[] { "id", "active", "created_at", "created_by", "quiniela_id", "updated_at", "updated_by", "user_id" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 1, null, null, 1 },
                    { 2, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 2, null, null, 1 },
                    { 3, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 3, null, null, 1 },
                    { 4, true, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 2, null, null, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_quinielas_quiniela_id",
                table: "usuarios_quinielas",
                column: "quiniela_id");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_quinielas_user_id_quiniela_id",
                table: "usuarios_quinielas",
                columns: new[] { "user_id", "quiniela_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "usuarios_quinielas");
        }
    }
}

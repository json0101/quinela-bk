using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quinela.Infrastructure.MigrationsUserApp
{
    /// <summary>
    /// Agrega al menú (esquema 'sec') la screen "Calendario" como hija del padre
    /// "Mundial 2026" (screen_id 1) y concede acceso al rol Administrador (role_id 1).
    /// order = 0 para que aparezca primero dentro del grupo.
    /// </summary>
    public partial class AddQuinelaMenuCalendario : Migration
    {
        // Fecha fija para datos de seed (UTC por Npgsql).
        private static readonly DateTime Seed = new DateTime(2026, 6, 8, 0, 0, 0, DateTimeKind.Utc);

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Screen "Calendario" hija de "Mundial 2026" (screen_father_id = 1).
            migrationBuilder.InsertData(
                schema: "sec",
                table: "screens",
                columns: new[] { "screen_id", "name", "route", "screen_father_id", "order", "is_father", "application_id", "created_at", "created_by", "updated_at", "updated_by", "active" },
                values: new object[] { 6, "Calendario", "/quinela/calendario", 1, 0, false, 1, Seed, "seed", null, null, true });

            // Acceso del rol Administrador (role_id = 1) a la nueva screen.
            migrationBuilder.InsertData(
                schema: "sec",
                table: "roles_screens",
                columns: new[] { "role_screen_id", "role_id", "screen_id", "created_at", "created_by", "updated_at", "updated_by", "active" },
                values: new object[] { 6, 1, 6, Seed, "seed", null, null, true });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(schema: "sec", table: "roles_screens", keyColumn: "role_screen_id", keyValue: 6);
            migrationBuilder.DeleteData(schema: "sec", table: "screens", keyColumn: "screen_id", keyValue: 6);
        }
    }
}

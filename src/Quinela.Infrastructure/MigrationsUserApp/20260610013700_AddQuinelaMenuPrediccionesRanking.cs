using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quinela.Infrastructure.MigrationsUserApp
{
    /// <summary>
    /// Agrega al menú (esquema 'sec') las screens "Predicciones" y "Ranking" como hijas
    /// del padre "Mundial 2026" (screen_id 1) y concede acceso al rol Administrador (role_id 1).
    /// </summary>
    public partial class AddQuinelaMenuPrediccionesRanking : Migration
    {
        // Fecha fija para datos de seed (UTC por Npgsql).
        private static readonly DateTime Seed = new DateTime(2026, 6, 8, 0, 0, 0, DateTimeKind.Utc);

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Screens hijas de "Mundial 2026" (screen_father_id = 1).
            migrationBuilder.InsertData(
                schema: "sec",
                table: "screens",
                columns: new[] { "screen_id", "name", "route", "screen_father_id", "order", "is_father", "application_id", "created_at", "created_by", "updated_at", "updated_by", "active" },
                values: new object[,]
                {
                    { 4, "Predicciones", "/quinela/predicciones", 1, 3, false, 1, Seed, "seed", null, null, true },
                    { 5, "Ranking", "/quinela/ranking", 1, 4, false, 1, Seed, "seed", null, null, true }
                });

            // Acceso del rol Administrador (role_id = 1) a las nuevas screens.
            migrationBuilder.InsertData(
                schema: "sec",
                table: "roles_screens",
                columns: new[] { "role_screen_id", "role_id", "screen_id", "created_at", "created_by", "updated_at", "updated_by", "active" },
                values: new object[,]
                {
                    { 4, 1, 4, Seed, "seed", null, null, true },
                    { 5, 1, 5, Seed, "seed", null, null, true }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(schema: "sec", table: "roles_screens", keyColumn: "role_screen_id", keyValue: 4);
            migrationBuilder.DeleteData(schema: "sec", table: "roles_screens", keyColumn: "role_screen_id", keyValue: 5);
            migrationBuilder.DeleteData(schema: "sec", table: "screens", keyColumn: "screen_id", keyValue: 4);
            migrationBuilder.DeleteData(schema: "sec", table: "screens", keyColumn: "screen_id", keyValue: 5);
        }
    }
}

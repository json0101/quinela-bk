using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quinela.Infrastructure.MigrationsUserApp
{
    /// <summary>
    /// Precarga en el esquema 'sec': la aplicación "Quinela", sus screens (menú),
    /// un rol administrador, el usuario admin (clave temporal "123") y sus enlaces.
    /// </summary>
    public partial class SeedQuinelaUserApp : Migration
    {
        // Fecha fija para datos de seed (UTC por Npgsql).
        private static readonly DateTime Seed = new DateTime(2026, 6, 8, 0, 0, 0, DateTimeKind.Utc);

        // Hash PBKDF2 (ASP.NET Core Identity) de la clave "123". CAMBIAR EN PRODUCCIÓN.
        private const string AdminPasswordHash =
            "AQAAAAEAAYagAAAAEImx3oI/A5eoczJHmOmaGpMBsUXQPnViwWHt2FB0bsi91/uI8AuSII818A0VMZ7tDA==";

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1) Aplicación
            migrationBuilder.InsertData(
                schema: "sec",
                table: "applications",
                columns: new[] { "application_id", "description", "created_at", "created_by", "updated_at", "updated_by", "active" },
                values: new object[] { 1, "Quinela", Seed, "seed", null, null, true });

            // 2) Screens (padre "Mundial 2026" + hijos Grupos/Equipos)
            migrationBuilder.InsertData(
                schema: "sec",
                table: "screens",
                columns: new[] { "screen_id", "name", "route", "screen_father_id", "order", "is_father", "application_id", "created_at", "created_by", "updated_at", "updated_by", "active" },
                values: new object[,]
                {
                    { 1, "Mundial 2026", "#", null, 1, true, 1, Seed, "seed", null, null, true },
                    { 2, "Grupos", "/quinela/master/grupos", 1, 1, false, 1, Seed, "seed", null, null, true },
                    { 3, "Equipos", "/quinela/master/equipos", 1, 2, false, 1, Seed, "seed", null, null, true }
                });

            // 3) Rol administrador de la app Quinela
            migrationBuilder.InsertData(
                schema: "sec",
                table: "roles",
                columns: new[] { "role_id", "application_id", "description", "created_at", "created_by", "updated_at", "updated_by", "active" },
                values: new object[] { 1, 1, "Administrador", Seed, "seed", null, null, true });

            // 4) Usuario admin (clave temporal "123")
            migrationBuilder.InsertData(
                schema: "sec",
                table: "users",
                columns: new[] { "user_id", "username", "password", "email", "country_id", "address_id", "employee_code", "created_at", "created_by", "updated_at", "updated_by", "active" },
                values: new object[] { 1, "Jason Hernandez", AdminPasswordHash, "jasonhernandezaguilar@gmail.com", null, null, "ADMIN", Seed, "seed", null, null, true });

            // 5) Rol -> Screens (acceso del rol admin a las 3 screens)
            migrationBuilder.InsertData(
                schema: "sec",
                table: "roles_screens",
                columns: new[] { "role_screen_id", "role_id", "screen_id", "created_at", "created_by", "updated_at", "updated_by", "active" },
                values: new object[,]
                {
                    { 1, 1, 1, Seed, "seed", null, null, true },
                    { 2, 1, 2, Seed, "seed", null, null, true },
                    { 3, 1, 3, Seed, "seed", null, null, true }
                });

            // 6) Usuario -> Rol
            migrationBuilder.InsertData(
                schema: "sec",
                table: "users_roles",
                columns: new[] { "user_role_id", "role_id", "user_id", "created_at", "created_by", "updated_at", "updated_by", "active" },
                values: new object[] { 1, 1, 1, Seed, "seed", null, null, true });

            // 7) Usuario -> Aplicación
            migrationBuilder.InsertData(
                schema: "sec",
                table: "users_applications",
                columns: new[] { "user_application_id", "application_id", "user_id", "created_at", "created_by", "updated_at", "updated_by", "active" },
                values: new object[] { 1, 1, 1, Seed, "seed", null, null, true });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(schema: "sec", table: "users_applications", keyColumn: "user_application_id", keyValue: 1);
            migrationBuilder.DeleteData(schema: "sec", table: "users_roles", keyColumn: "user_role_id", keyValue: 1);
            migrationBuilder.DeleteData(schema: "sec", table: "roles_screens", keyColumn: "role_screen_id", keyValue: 1);
            migrationBuilder.DeleteData(schema: "sec", table: "roles_screens", keyColumn: "role_screen_id", keyValue: 2);
            migrationBuilder.DeleteData(schema: "sec", table: "roles_screens", keyColumn: "role_screen_id", keyValue: 3);
            migrationBuilder.DeleteData(schema: "sec", table: "users", keyColumn: "user_id", keyValue: 1);
            migrationBuilder.DeleteData(schema: "sec", table: "roles", keyColumn: "role_id", keyValue: 1);
            migrationBuilder.DeleteData(schema: "sec", table: "screens", keyColumn: "screen_id", keyValue: 2);
            migrationBuilder.DeleteData(schema: "sec", table: "screens", keyColumn: "screen_id", keyValue: 3);
            migrationBuilder.DeleteData(schema: "sec", table: "screens", keyColumn: "screen_id", keyValue: 1);
            migrationBuilder.DeleteData(schema: "sec", table: "applications", keyColumn: "application_id", keyValue: 1);
        }
    }
}

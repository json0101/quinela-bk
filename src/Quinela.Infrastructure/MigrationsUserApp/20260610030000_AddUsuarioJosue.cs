using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quinela.Infrastructure.MigrationsUserApp
{
    /// <summary>
    /// Precarga en el esquema 'sec' un nuevo usuario (josue20212015@gmail.com) con clave
    /// temporal "123", enlazado al rol Administrador (role_id 1) y a la app Quinela
    /// (application_id 1) para que pueda iniciar sesión. CAMBIAR CLAVE EN PRODUCCIÓN.
    /// </summary>
    public partial class AddUsuarioJosue : Migration
    {
        // Fecha fija para datos de seed (UTC por Npgsql).
        private static readonly DateTime Seed = new DateTime(2026, 6, 8, 0, 0, 0, DateTimeKind.Utc);

        // Hash PBKDF2 (ASP.NET Core Identity) de la clave "123". CAMBIAR EN PRODUCCIÓN.
        private const string UserPasswordHash =
            "AQAAAAEAAYagAAAAEImx3oI/A5eoczJHmOmaGpMBsUXQPnViwWHt2FB0bsi91/uI8AuSII818A0VMZ7tDA==";

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1) Usuario (clave temporal "123")
            migrationBuilder.InsertData(
                schema: "sec",
                table: "users",
                columns: new[] { "user_id", "username", "password", "email", "country_id", "address_id", "employee_code", "created_at", "created_by", "updated_at", "updated_by", "active" },
                values: new object[] { 2, "Josue", UserPasswordHash, "josue20212015@gmail.com", null, null, "JOSUE", Seed, "seed", null, null, true });

            // 2) Usuario -> Rol Administrador (role_id = 1)
            migrationBuilder.InsertData(
                schema: "sec",
                table: "users_roles",
                columns: new[] { "user_role_id", "role_id", "user_id", "created_at", "created_by", "updated_at", "updated_by", "active" },
                values: new object[] { 2, 1, 2, Seed, "seed", null, null, true });

            // 3) Usuario -> Aplicación Quinela (application_id = 1)
            migrationBuilder.InsertData(
                schema: "sec",
                table: "users_applications",
                columns: new[] { "user_application_id", "application_id", "user_id", "created_at", "created_by", "updated_at", "updated_by", "active" },
                values: new object[] { 2, 1, 2, Seed, "seed", null, null, true });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(schema: "sec", table: "users_applications", keyColumn: "user_application_id", keyValue: 2);
            migrationBuilder.DeleteData(schema: "sec", table: "users_roles", keyColumn: "user_role_id", keyValue: 2);
            migrationBuilder.DeleteData(schema: "sec", table: "users", keyColumn: "user_id", keyValue: 2);
        }
    }
}

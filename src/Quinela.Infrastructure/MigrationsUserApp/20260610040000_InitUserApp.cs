using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Quinela.Infrastructure.MigrationsUserApp
{
    /// <summary>
    /// Migración inicial consolidada del esquema 'sec' (UserApp.Domain 1.0.3):
    /// crea TODAS las tablas (incluida users_login_logs) y precarga los datos de Quinela
    /// (aplicación, screens/menú, rol admin, usuarios admin + josue y sus enlaces).
    /// Reemplaza a las migraciones previas (InitUserAppSchema, SeedQuinelaUserApp, menús, AddUsuarioJosue).
    /// </summary>
    public partial class InitUserApp : Migration
    {
        // Fecha fija para datos de seed (UTC por Npgsql).
        private static readonly DateTime Seed = new DateTime(2026, 6, 8, 0, 0, 0, DateTimeKind.Utc);

        // Hash PBKDF2 (ASP.NET Core Identity) de la clave "123". CAMBIAR EN PRODUCCIÓN.
        private const string PasswordHash =
            "AQAAAAEAAYagAAAAEImx3oI/A5eoczJHmOmaGpMBsUXQPnViwWHt2FB0bsi91/uI8AuSII818A0VMZ7tDA==";

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // =================== ESQUEMA ===================
            migrationBuilder.EnsureSchema(name: "sec");

            migrationBuilder.CreateTable(
                name: "actions",
                schema: "sec",
                columns: table => new
                {
                    action_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "VARCHAR", maxLength: 250, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "VARCHAR", maxLength: 250, nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_actions", x => x.action_id);
                });

            migrationBuilder.CreateTable(
                name: "applications",
                schema: "sec",
                columns: table => new
                {
                    application_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "VARCHAR", maxLength: 250, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "VARCHAR", maxLength: 250, nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_applications", x => x.application_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "sec",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    country_id = table.Column<int>(type: "integer", nullable: true),
                    address_id = table.Column<int>(type: "integer", nullable: true),
                    employee_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "VARCHAR", maxLength: 250, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "VARCHAR", maxLength: 250, nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                schema: "sec",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: false),
                    application_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "VARCHAR", maxLength: 250, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "VARCHAR", maxLength: 250, nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.role_id);
                    table.ForeignKey(
                        name: "FK_roles_applications_application_id",
                        column: x => x.application_id,
                        principalSchema: "sec",
                        principalTable: "applications",
                        principalColumn: "application_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "screens",
                schema: "sec",
                columns: table => new
                {
                    screen_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: false),
                    route = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: false),
                    screen_father_id = table.Column<int>(type: "integer", nullable: true),
                    order = table.Column<int>(type: "integer", nullable: false),
                    is_father = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    application_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "VARCHAR", maxLength: 250, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "VARCHAR", maxLength: 250, nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_screens", x => x.screen_id);
                    table.ForeignKey(
                        name: "FK_screens_applications_application_id",
                        column: x => x.application_id,
                        principalSchema: "sec",
                        principalTable: "applications",
                        principalColumn: "application_id");
                    table.ForeignKey(
                        name: "FK_screens_screens_screen_father_id",
                        column: x => x.screen_father_id,
                        principalSchema: "sec",
                        principalTable: "screens",
                        principalColumn: "screen_id");
                });

            migrationBuilder.CreateTable(
                name: "users_applications",
                schema: "sec",
                columns: table => new
                {
                    user_application_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    application_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "VARCHAR", maxLength: 250, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "VARCHAR", maxLength: 250, nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users_applications", x => x.user_application_id);
                    table.ForeignKey(
                        name: "FK_users_applications_applications_application_id",
                        column: x => x.application_id,
                        principalSchema: "sec",
                        principalTable: "applications",
                        principalColumn: "application_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_users_applications_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "sec",
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users_roles",
                schema: "sec",
                columns: table => new
                {
                    user_role_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "VARCHAR", maxLength: 250, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "VARCHAR", maxLength: 250, nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users_roles", x => x.user_role_id);
                    table.ForeignKey(
                        name: "FK_users_roles_roles_role_id",
                        column: x => x.role_id,
                        principalSchema: "sec",
                        principalTable: "roles",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_users_roles_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "sec",
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "roles_screens",
                schema: "sec",
                columns: table => new
                {
                    role_screen_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    screen_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "VARCHAR", maxLength: 250, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "VARCHAR", maxLength: 250, nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles_screens", x => x.role_screen_id);
                    table.ForeignKey(
                        name: "FK_roles_screens_roles_role_id",
                        column: x => x.role_id,
                        principalSchema: "sec",
                        principalTable: "roles",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_roles_screens_screens_screen_id",
                        column: x => x.screen_id,
                        principalSchema: "sec",
                        principalTable: "screens",
                        principalColumn: "screen_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "screens_actions",
                schema: "sec",
                columns: table => new
                {
                    user_application_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    screen_id = table.Column<int>(type: "integer", nullable: false),
                    action_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "VARCHAR", maxLength: 250, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "VARCHAR", maxLength: 250, nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_screens_actions", x => x.user_application_id);
                    table.ForeignKey(
                        name: "FK_screens_actions_actions_action_id",
                        column: x => x.action_id,
                        principalSchema: "sec",
                        principalTable: "actions",
                        principalColumn: "action_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_screens_actions_screens_screen_id",
                        column: x => x.screen_id,
                        principalSchema: "sec",
                        principalTable: "screens",
                        principalColumn: "screen_id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Nueva en UserApp.Domain 1.0.3: bitácora de inicios de sesión.
            migrationBuilder.CreateTable(
                name: "users_login_logs",
                schema: "sec",
                columns: table => new
                {
                    user_login_log_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: true),
                    application_id = table.Column<int>(type: "integer", nullable: false),
                    successful = table.Column<bool>(type: "boolean", nullable: false),
                    ip_address = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    failure_reason = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    login_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_agent = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    username = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users_login_logs", x => x.user_login_log_id);
                    table.ForeignKey(
                        name: "FK_users_login_logs_applications_application_id",
                        column: x => x.application_id,
                        principalSchema: "sec",
                        principalTable: "applications",
                        principalColumn: "application_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_users_login_logs_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "sec",
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            // =================== ÍNDICES ===================
            migrationBuilder.CreateIndex(name: "IX_roles_application_id", schema: "sec", table: "roles", column: "application_id");
            migrationBuilder.CreateIndex(name: "IX_roles_screens_role_id", schema: "sec", table: "roles_screens", column: "role_id");
            migrationBuilder.CreateIndex(name: "IX_roles_screens_screen_id", schema: "sec", table: "roles_screens", column: "screen_id");
            migrationBuilder.CreateIndex(name: "IX_screens_application_id", schema: "sec", table: "screens", column: "application_id");
            migrationBuilder.CreateIndex(name: "IX_screens_screen_father_id", schema: "sec", table: "screens", column: "screen_father_id");
            migrationBuilder.CreateIndex(name: "IX_screens_actions_action_id", schema: "sec", table: "screens_actions", column: "action_id");
            migrationBuilder.CreateIndex(name: "IX_screens_actions_screen_id", schema: "sec", table: "screens_actions", column: "screen_id");
            migrationBuilder.CreateIndex(name: "IX_users_applications_application_id", schema: "sec", table: "users_applications", column: "application_id");
            migrationBuilder.CreateIndex(name: "IX_users_applications_user_id", schema: "sec", table: "users_applications", column: "user_id");
            migrationBuilder.CreateIndex(name: "IX_users_login_logs_application_id", schema: "sec", table: "users_login_logs", column: "application_id");
            migrationBuilder.CreateIndex(name: "IX_users_login_logs_user_id", schema: "sec", table: "users_login_logs", column: "user_id");
            migrationBuilder.CreateIndex(name: "IX_users_roles_role_id", schema: "sec", table: "users_roles", column: "role_id");
            migrationBuilder.CreateIndex(name: "IX_users_roles_user_id", schema: "sec", table: "users_roles", column: "user_id");

            // =================== SEED ===================
            // 1) Aplicaciones: UserApp (id 1, administra permisos) y Quinela (id 2).
            migrationBuilder.InsertData(
                schema: "sec",
                table: "applications",
                columns: new[] { "application_id", "description", "created_at", "created_by", "updated_at", "updated_by", "active" },
                values: new object[,]
                {
                    { 1, "UserApp", Seed, "seed", null, null, true },
                    { 2, "Quinela", Seed, "seed", null, null, true }
                });

            // 2) Usuarios (clave temporal "123")
            migrationBuilder.InsertData(
                schema: "sec",
                table: "users",
                columns: new[] { "user_id", "username", "password", "email", "country_id", "address_id", "employee_code", "created_at", "created_by", "updated_at", "updated_by", "active" },
                values: new object[,]
                {
                    { 1, "jason.hernandez", PasswordHash, "jasonhernandezaguilar@gmail.com", null, null, "ADMIN", Seed, "seed", null, null, true },
                    { 2, "elmer.romero", PasswordHash, "josue20212015@gmail.com", null, null, "ELMER", Seed, "seed", null, null, true }
                });

            // 3) Roles: uno por aplicación.
            migrationBuilder.InsertData(
                schema: "sec",
                table: "roles",
                columns: new[] { "role_id", "application_id", "description", "created_at", "created_by", "updated_at", "updated_by", "active" },
                values: new object[,]
                {
                    { 1, 1, "Admin UserApp", Seed, "seed", null, null, true },
                    { 2, 2, "Admin Quinela", Seed, "seed", null, null, true }
                });

            // 4) Screens. Padres primero (FK self-ref). UserApp (app 1) bajo "Seguridad";
            //    Quinela (app 2) bajo "Mundial 2026".
            migrationBuilder.InsertData(
                schema: "sec",
                table: "screens",
                columns: new[] { "screen_id", "name", "route", "screen_father_id", "order", "is_father", "application_id", "created_at", "created_by", "updated_at", "updated_by", "active" },
                values: new object[,]
                {
                    // --- UserApp (application_id = 1) ---
                    { 1, "Seguridad", "#", null, 1, true, 1, Seed, "seed", null, null, true },
                    { 2, "Aplicaciones", "/sec/app", 1, 1, false, 1, Seed, "seed", null, null, true },
                    { 3, "Usuarios", "/sec/user", 1, 2, false, 1, Seed, "seed", null, null, true },
                    { 4, "Roles", "/sec/role", 1, 3, false, 1, Seed, "seed", null, null, true },
                    { 5, "Pantallas", "/sec/screen", 1, 4, false, 1, Seed, "seed", null, null, true },
                    { 6, "Acciones", "/sec/action", 1, 5, false, 1, Seed, "seed", null, null, true },
                    { 7, "Acciones-Pantalla", "/sec/action-screen", 1, 6, false, 1, Seed, "seed", null, null, true },
                    { 8, "Pantallas-Rol", "/sec/role-screen", 1, 7, false, 1, Seed, "seed", null, null, true },
                    { 9, "Usuarios-Aplicación", "/sec/user-application", 1, 8, false, 1, Seed, "seed", null, null, true },
                    { 10, "Árbol de Accesos", "/sec/tree-access", 1, 9, false, 1, Seed, "seed", null, null, true },
                    { 11, "Árbol de Pantallas", "/sec/tree-screen", 1, 10, false, 1, Seed, "seed", null, null, true },
                    // --- Quinela (application_id = 2) ---
                    // Padre "Mundial 2026": vistas del torneo en curso.
                    { 12, "Mundial 2026", "#", null, 1, true, 2, Seed, "seed", null, null, true },
                    { 17, "Calendario", "/quinela/calendario", 12, 1, false, 2, Seed, "seed", null, null, true },
                    { 16, "Ranking", "/quinela/ranking", 12, 2, false, 2, Seed, "seed", null, null, true },
                    { 24, "Live", "/quinela/live", 12, 3, false, 2, Seed, "seed", null, null, true },
                    { 25, "Tabla de Grupos", "/quinela/grupos", 12, 4, false, 2, Seed, "seed", null, null, true },
                    // Padre "Maestros": CRUD de catálogo (el usuario necesita acceso al padre para ver los hijos).
                    { 21, "Maestros", "#", null, 2, true, 2, Seed, "seed", null, null, true },
                    { 13, "Grupos", "/quinela/master/grupos", 21, 1, false, 2, Seed, "seed", null, null, true },
                    { 14, "Equipos", "/quinela/master/equipos", 21, 2, false, 2, Seed, "seed", null, null, true },
                    { 18, "Quinielas", "/quinela/master/quinielas", 21, 3, false, 2, Seed, "seed", null, null, true },
                    { 19, "Torneos", "/quinela/master/torneos", 21, 4, false, 2, Seed, "seed", null, null, true },
                    { 20, "Partidos", "/quinela/master/partidos", 21, 5, false, 2, Seed, "seed", null, null, true },
                    { 22, "Tipos de Partido", "/quinela/master/tipos-partido", 21, 6, false, 2, Seed, "seed", null, null, true },
                    { 23, "Usuarios - Quinelas", "/quinela/master/usuarios-quinielas", 21, 7, false, 2, Seed, "seed", null, null, true },
                    { 26, "Fases", "/quinela/master/fases", 21, 8, false, 2, Seed, "seed", null, null, true }
                });

            // 5) Rol -> Screens. "Admin UserApp" (1) ve las screens de UserApp; "Admin Quinela" (2) las de Quinela.
            migrationBuilder.InsertData(
                schema: "sec",
                table: "roles_screens",
                columns: new[] { "role_screen_id", "role_id", "screen_id", "created_at", "created_by", "updated_at", "updated_by", "active" },
                values: new object[,]
                {
                    { 1, 1, 1, Seed, "seed", null, null, true },
                    { 2, 1, 2, Seed, "seed", null, null, true },
                    { 3, 1, 3, Seed, "seed", null, null, true },
                    { 4, 1, 4, Seed, "seed", null, null, true },
                    { 5, 1, 5, Seed, "seed", null, null, true },
                    { 6, 1, 6, Seed, "seed", null, null, true },
                    { 7, 1, 7, Seed, "seed", null, null, true },
                    { 8, 1, 8, Seed, "seed", null, null, true },
                    { 9, 1, 9, Seed, "seed", null, null, true },
                    { 10, 1, 10, Seed, "seed", null, null, true },
                    { 11, 1, 11, Seed, "seed", null, null, true },
                    { 12, 2, 12, Seed, "seed", null, null, true },
                    { 13, 2, 13, Seed, "seed", null, null, true },
                    { 14, 2, 14, Seed, "seed", null, null, true },
                    { 16, 2, 16, Seed, "seed", null, null, true },
                    { 17, 2, 17, Seed, "seed", null, null, true },
                    { 18, 2, 18, Seed, "seed", null, null, true },
                    { 19, 2, 19, Seed, "seed", null, null, true },
                    { 20, 2, 20, Seed, "seed", null, null, true },
                    { 21, 2, 21, Seed, "seed", null, null, true },
                    { 22, 2, 22, Seed, "seed", null, null, true },
                    { 23, 2, 23, Seed, "seed", null, null, true },
                    { 24, 2, 24, Seed, "seed", null, null, true },
                    { 25, 2, 25, Seed, "seed", null, null, true },
                    { 26, 2, 26, Seed, "seed", null, null, true }
                });

            // 6) Usuario -> Rol. Ambos usuarios reciben AMBOS roles (acceso total a las dos apps).
            migrationBuilder.InsertData(
                schema: "sec",
                table: "users_roles",
                columns: new[] { "user_role_id", "role_id", "user_id", "created_at", "created_by", "updated_at", "updated_by", "active" },
                values: new object[,]
                {
                    { 1, 1, 1, Seed, "seed", null, null, true },
                    { 2, 2, 1, Seed, "seed", null, null, true },
                    { 3, 1, 2, Seed, "seed", null, null, true },
                    { 4, 2, 2, Seed, "seed", null, null, true }
                });

            // 7) Usuario -> Aplicación. Ambos usuarios a las dos apps.
            migrationBuilder.InsertData(
                schema: "sec",
                table: "users_applications",
                columns: new[] { "user_application_id", "application_id", "user_id", "created_at", "created_by", "updated_at", "updated_by", "active" },
                values: new object[,]
                {
                    { 1, 1, 1, Seed, "seed", null, null, true },
                    { 2, 2, 1, Seed, "seed", null, null, true },
                    { 3, 1, 2, Seed, "seed", null, null, true },
                    { 4, 2, 2, Seed, "seed", null, null, true }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "roles_screens", schema: "sec");
            migrationBuilder.DropTable(name: "screens_actions", schema: "sec");
            migrationBuilder.DropTable(name: "users_applications", schema: "sec");
            migrationBuilder.DropTable(name: "users_login_logs", schema: "sec");
            migrationBuilder.DropTable(name: "users_roles", schema: "sec");
            migrationBuilder.DropTable(name: "actions", schema: "sec");
            migrationBuilder.DropTable(name: "screens", schema: "sec");
            migrationBuilder.DropTable(name: "roles", schema: "sec");
            migrationBuilder.DropTable(name: "users", schema: "sec");
            migrationBuilder.DropTable(name: "applications", schema: "sec");
        }
    }
}

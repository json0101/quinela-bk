using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quinela.Infrastructure.MigrationsUserApp
{
    /// <summary>
    /// Perfil "Jugador" (rol de la app Quinela, application_id 2): acceso a todo Quiniela
    /// MENOS los masters (Grupos/Equipos -> solo Administradores). Precarga los 29 jugadores
    /// del Excel "Usuarios Quiniela" con su clave hasheada (PBKDF2) y los asocia al rol Jugador
    /// y a la app Quinela. Los administradores (roles 1-2, usuarios 1-2) se mantienen intactos.
    /// </summary>
    public partial class SeedJugadores : Migration
    {
        private static readonly DateTime Seed = new DateTime(2026, 6, 11, 0, 0, 0, DateTimeKind.Utc);

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1) Rol Jugador (app Quinela = 2)
            migrationBuilder.InsertData(
                schema: "sec",
                table: "roles",
                columns: new[] { "role_id", "application_id", "description", "created_at", "created_by", "updated_at", "updated_by", "active" },
                values: new object[] { 3, 2, "Jugador", Seed, "seed", null, null, true });

            // 2) Usuarios (clave del Excel, hasheada con PBKDF2)
            migrationBuilder.InsertData(
                schema: "sec",
                table: "users",
                columns: new[] { "user_id", "username", "password", "email", "country_id", "address_id", "employee_code", "created_at", "created_by", "updated_at", "updated_by", "active" },
                values: new object[,]
                {
                    { 3, "ludin.castro", "AQAAAAEAAYagAAAAEGIsNdT/st7aOxFu8QCxmYhkHjXtHBqPlVSM/bu95JCx1HH8L0kc89GFFGAMzauoeg==", "ludin.maradiaga@gmail.com", null, null, "JUG03", Seed, "seed", null, null, true },
                    { 4, "kevin.dubon", "AQAAAAEAAYagAAAAEGJXsjVP+9vMW08kcYBx/GEslZKGpsLhFNfDkcWOB33FbuE70SdiX2h43IKowboJIw==", "kevinduvon@icloud.com", null, null, "JUG04", Seed, "seed", null, null, true },
                    { 5, "daniel.maradiaga", "AQAAAAEAAYagAAAAEJqT6LydTLU9ru3EsgOcq7cfq/lSnQEECyiwk8fabSzKr96jQ0/JPbMED1uogvDhng==", "danielmaradiaga500@gmail.com", null, null, "JUG05", Seed, "seed", null, null, true },
                    { 6, "enrique.bonilla", "AQAAAAEAAYagAAAAELfejZGu+s35AmEvhWmL+OXnCY67eTU+tG0g/sBXdmrv9q02k8TUX4uZVUgHKEAaRA==", "enrique.bonilla@quiniela.com", null, null, "JUG06", Seed, "seed", null, null, true },
                    { 7, "osman.rivas", "AQAAAAEAAYagAAAAEKux52hiWzC0fM4u6JB7X5Pj7gR+s2Af5zs76aaoBbjqX5lhSXRgjMZ00m/1nJElGg==", "danielosman504@gmail.com", null, null, "JUG07", Seed, "seed", null, null, true },
                    { 8, "juana.garacia", "AQAAAAEAAYagAAAAEKehuBd/wU9G0L8KkPrJhoXhFx31ry0vb02fpABfXEybF4aTIEaAtujMQK5nT91b1A==", "oneydagarcia30@yahoo.com", null, null, "JUG08", Seed, "seed", null, null, true },
                    { 9, "pedro.rodriguez", "AQAAAAEAAYagAAAAEBBBvSoH7a55JDK/4nVsV4W4RvnoYQsi3zlOrZ9izBHcGJiA6cqCMbAqQk71DZbtCQ==", "guezpa11824@gmail.com", null, null, "JUG09", Seed, "seed", null, null, true },
                    { 10, "oscar.lopez", "AQAAAAEAAYagAAAAEAIfzfgIEKKVYN4dUn7UggXOqphf84H11K0VPFKnEc///vB7OGmic2nEwvaII2Hpig==", "lopez.oscar4311@gmail.com", null, null, "JUG10", Seed, "seed", null, null, true },
                    { 11, "fernando.inestroza", "AQAAAAEAAYagAAAAEEzSLmOJ8rbxQuS4Hn2Y7fG8XkgJSnZLY9Ikej7ZvM6a0NKra0S215MQwhWRM9k7dw==", "Ferinestroza3@hotmail.com", null, null, "JUG11", Seed, "seed", null, null, true },
                    { 12, "calin.inestroza", "AQAAAAEAAYagAAAAEP7kEO1NRZaDq+FKIVOGbjtQFe0pEaeyEc97n5ygkf+eW8lf5fM3b+4UkuHjl7kvog==", "cinestroza0@gmail.com", null, null, "JUG12", Seed, "seed", null, null, true },
                    { 13, "carlos.inestroza", "AQAAAAEAAYagAAAAELAr6/cS527sDL5TvyMAD69eTUi+voEcvEGTZJ+y/FEY/FT1Lfmw11nzikSqhHiKBQ==", "ca_inestroza@hotmail.com", null, null, "JUG13", Seed, "seed", null, null, true },
                    { 14, "cesia.zavala", "AQAAAAEAAYagAAAAEA3ZZQzmJzcT+E6xLsEXWU9TSwp6wlQzIElEqpWMoGa8HK3ML7tclob0oiVqdvhdqQ==", "czabigail@icloud.com", null, null, "JUG14", Seed, "seed", null, null, true },
                    { 15, "katherine.hernandez", "AQAAAAEAAYagAAAAENrj5+pM1Ky+BQCxIUVO7MNQeW8tnUKnjbur+s4EdnZF4zdeY9wDSMnzG6TU/FJ5OA==", "abihrz2019@gmail.com", null, null, "JUG15", Seed, "seed", null, null, true },
                    { 16, "sarahi.mendez", "AQAAAAEAAYagAAAAEC/VQ7zk2lqN0+zqJD0RvauViYXypBnH+Od9MzzltWM4mAmMkGi/+KFmRRoaiWx73A==", "sarahymendez1989@gmail.com", null, null, "JUG16", Seed, "seed", null, null, true },
                    { 17, "dania.avila", "AQAAAAEAAYagAAAAEE4cil9596Ow73E1byuXXjem0YSt7W3ae+JriMYUxiWlYU7CRm0bY4W95Q1Eqf3o5Q==", "dania_sofia_p@hotmail.com", null, null, "JUG17", Seed, "seed", null, null, true },
                    { 18, "wendy.valladares", "AQAAAAEAAYagAAAAEOLtqua7u/ouZ7bqKqZBAV8iKlIZ/tixblvbGrXs19lfxpO69/oe53biX6+hIrvApQ==", "wendyvalladares12@icloud.com", null, null, "JUG18", Seed, "seed", null, null, true },
                    { 19, "alex.giron", "AQAAAAEAAYagAAAAELg3aHyFljMD7dYwwaIiZp999VRBKEoQuHUhhh8HzUPufEQ3MUQq80Tj0NS+cnFT8w==", "alegiron_93@hotmail.com", null, null, "JUG19", Seed, "seed", null, null, true },
                    { 20, "jeavanny.zambrano", "AQAAAAEAAYagAAAAEDS0gZ89WWH85UvdBXMSQr70K2pVaALw3TZxe2x1YgihbVPvQBpO4KsJWjwQHgW9Bw==", "alexzambrano0613@gmail.com", null, null, "JUG20", Seed, "seed", null, null, true },
                    { 21, "osman.mendez", "AQAAAAEAAYagAAAAENK8MJ6cgdDHkdg5OMkSMumXuw05OPIESkESO7fQGZiRYi60gnkfEbK5ULiACGWoZg==", "osman.mendez@gmail.com", null, null, "JUG21", Seed, "seed", null, null, true },
                    { 22, "carlos.funez", "AQAAAAEAAYagAAAAEOuGcnXMXkZ79yZTKno0ROR0iTZZbKaiOkDkM9MnXeOrkHlJbIt3R5BP8t/936OXcg==", "carlos.funez@tegraglobal.com", null, null, "JUG22", Seed, "seed", null, null, true },
                    { 23, "elder.osorto", "AQAAAAEAAYagAAAAEJIBR9PPmvJzLdYrPzQoQjZRCen2DFqjDe3tYslgWTNqVOOI827DoAzCBJri5RYy1Q==", "eosorto21@hotmail.com", null, null, "JUG23", Seed, "seed", null, null, true },
                    { 24, "maudiel.teruel", "AQAAAAEAAYagAAAAEI5Uh/Ec3RKAdmRxUpSxzK0In6M6aU0+GUrc4zXAQ4nAjPALuM5PeJjHrE4sSKx20A==", "maudiel.teruel99@gmail.com", null, null, "JUG24", Seed, "seed", null, null, true },
                    { 25, "erick.morales", "AQAAAAEAAYagAAAAEFF/f41YPyWWj+2+K9FmFReLgFijG9WLtvnOvhdVd7iwHYz5JMQbBy5Wm2K9wfhOzw==", "ericknoel558@gmail.com", null, null, "JUG25", Seed, "seed", null, null, true },
                    { 26, "alejandra.urbina", "AQAAAAEAAYagAAAAECvdECZbpio8j3hAdfQaygmYy1BIMyi8dFt84/CMCl5uzHDQJiOtfYO08hA3nObOUw==", "alejandra_urbina_15@hotmail.com", null, null, "JUG26", Seed, "seed", null, null, true },
                    { 27, "yadira.villanueva", "AQAAAAEAAYagAAAAEPYUs04iNk2HZ2EmVLJ3h9lMe30nji0gikvG79t93T79nuzSv27yqe0wpGoLloy78g==", "yadiramarisolv@outlook.com", null, null, "JUG27", Seed, "seed", null, null, true },
                    { 28, "militza.contreras", "AQAAAAEAAYagAAAAEH7tUqyYJSJWp6rwnr3hgvYrmwgyWhkyez69Vjsi1J3ZS9uV2jejs+TvZhkib8S7Ww==", "milidanna15@gmail.com", null, null, "JUG28", Seed, "seed", null, null, true },
                    { 29, "rocio.solis", "AQAAAAEAAYagAAAAEG8Qnwxw36M8BV1n4AHrmwH7n1YKK2qd8rjBJ53cFXEa+XxUTfKw2BgD/rRdm07Y8g==", "rociosolis465@gmail.com", null, null, "JUG29", Seed, "seed", null, null, true },
                    { 30, "yolani.tinoco", "AQAAAAEAAYagAAAAEMbxEFEa2hh+TGxYhj4e/zJnl1fuSTMDi+1UBKRwnPbg0vzIzSRqmSEjrq9LbMUCtw==", "tinocoyolani08@gmail.com", null, null, "JUG30", Seed, "seed", null, null, true },
                    { 31, "william.palma", "AQAAAAEAAYagAAAAEEjfvY/NQiR63zTMlHu52tj8xBVkTYzo5jORLDDfJo7VZDg70CjnBHfNp62MuI5Ieg==", "wp98273725@gmail.com", null, null, "JUG31", Seed, "seed", null, null, true }
                });

            // 3) Usuario -> Rol Jugador
            migrationBuilder.InsertData(
                schema: "sec",
                table: "users_roles",
                columns: new[] { "user_role_id", "role_id", "user_id", "created_at", "created_by", "updated_at", "updated_by", "active" },
                values: new object[,]
                {
                    { 5, 3, 3, Seed, "seed", null, null, true },
                    { 6, 3, 4, Seed, "seed", null, null, true },
                    { 7, 3, 5, Seed, "seed", null, null, true },
                    { 8, 3, 6, Seed, "seed", null, null, true },
                    { 9, 3, 7, Seed, "seed", null, null, true },
                    { 10, 3, 8, Seed, "seed", null, null, true },
                    { 11, 3, 9, Seed, "seed", null, null, true },
                    { 12, 3, 10, Seed, "seed", null, null, true },
                    { 13, 3, 11, Seed, "seed", null, null, true },
                    { 14, 3, 12, Seed, "seed", null, null, true },
                    { 15, 3, 13, Seed, "seed", null, null, true },
                    { 16, 3, 14, Seed, "seed", null, null, true },
                    { 17, 3, 15, Seed, "seed", null, null, true },
                    { 18, 3, 16, Seed, "seed", null, null, true },
                    { 19, 3, 17, Seed, "seed", null, null, true },
                    { 20, 3, 18, Seed, "seed", null, null, true },
                    { 21, 3, 19, Seed, "seed", null, null, true },
                    { 22, 3, 20, Seed, "seed", null, null, true },
                    { 23, 3, 21, Seed, "seed", null, null, true },
                    { 24, 3, 22, Seed, "seed", null, null, true },
                    { 25, 3, 23, Seed, "seed", null, null, true },
                    { 26, 3, 24, Seed, "seed", null, null, true },
                    { 27, 3, 25, Seed, "seed", null, null, true },
                    { 28, 3, 26, Seed, "seed", null, null, true },
                    { 29, 3, 27, Seed, "seed", null, null, true },
                    { 30, 3, 28, Seed, "seed", null, null, true },
                    { 31, 3, 29, Seed, "seed", null, null, true },
                    { 32, 3, 30, Seed, "seed", null, null, true },
                    { 33, 3, 31, Seed, "seed", null, null, true }
                });

            // 4) Usuario -> Aplicacion Quinela
            migrationBuilder.InsertData(
                schema: "sec",
                table: "users_applications",
                columns: new[] { "user_application_id", "application_id", "user_id", "created_at", "created_by", "updated_at", "updated_by", "active" },
                values: new object[,]
                {
                    { 5, 2, 3, Seed, "seed", null, null, true },
                    { 6, 2, 4, Seed, "seed", null, null, true },
                    { 7, 2, 5, Seed, "seed", null, null, true },
                    { 8, 2, 6, Seed, "seed", null, null, true },
                    { 9, 2, 7, Seed, "seed", null, null, true },
                    { 10, 2, 8, Seed, "seed", null, null, true },
                    { 11, 2, 9, Seed, "seed", null, null, true },
                    { 12, 2, 10, Seed, "seed", null, null, true },
                    { 13, 2, 11, Seed, "seed", null, null, true },
                    { 14, 2, 12, Seed, "seed", null, null, true },
                    { 15, 2, 13, Seed, "seed", null, null, true },
                    { 16, 2, 14, Seed, "seed", null, null, true },
                    { 17, 2, 15, Seed, "seed", null, null, true },
                    { 18, 2, 16, Seed, "seed", null, null, true },
                    { 19, 2, 17, Seed, "seed", null, null, true },
                    { 20, 2, 18, Seed, "seed", null, null, true },
                    { 21, 2, 19, Seed, "seed", null, null, true },
                    { 22, 2, 20, Seed, "seed", null, null, true },
                    { 23, 2, 21, Seed, "seed", null, null, true },
                    { 24, 2, 22, Seed, "seed", null, null, true },
                    { 25, 2, 23, Seed, "seed", null, null, true },
                    { 26, 2, 24, Seed, "seed", null, null, true },
                    { 27, 2, 25, Seed, "seed", null, null, true },
                    { 28, 2, 26, Seed, "seed", null, null, true },
                    { 29, 2, 27, Seed, "seed", null, null, true },
                    { 30, 2, 28, Seed, "seed", null, null, true },
                    { 31, 2, 29, Seed, "seed", null, null, true },
                    { 32, 2, 30, Seed, "seed", null, null, true },
                    { 33, 2, 31, Seed, "seed", null, null, true }
                });

            // 5) Rol Jugador -> Screens de Quinela MENOS los masters. Solo el grupo "Mundial 2026"
            //    (padre 12) con sus vistas: Ranking(16), Calendario(17), Live(24), Tabla de Grupos(25).
            //    Se EXCLUYE el grupo "Maestros" (21) y todos sus CRUD /quinela/master/* (solo Admin).
            migrationBuilder.InsertData(
                schema: "sec",
                table: "roles_screens",
                columns: new[] { "role_screen_id", "role_id", "screen_id", "created_at", "created_by", "updated_at", "updated_by", "active" },
                values: new object[,]
                {
                    { 26, 3, 12, Seed, "seed", null, null, true },
                    { 27, 3, 16, Seed, "seed", null, null, true },
                    { 28, 3, 17, Seed, "seed", null, null, true },
                    { 29, 3, 24, Seed, "seed", null, null, true },
                    { 30, 3, 25, Seed, "seed", null, null, true }
                });

            // 6) Reinicia las secuencias identity al MAX insertado (los inserts con id explicito
            //    no avanzan la secuencia; sin esto la app chocaria al crear registros nuevos).
            migrationBuilder.Sql("SELECT setval(pg_get_serial_sequence('sec.applications','application_id'), 2, true);");
            migrationBuilder.Sql("SELECT setval(pg_get_serial_sequence('sec.roles','role_id'), 3, true);");
            migrationBuilder.Sql("SELECT setval(pg_get_serial_sequence('sec.users','user_id'), 31, true);");
            migrationBuilder.Sql("SELECT setval(pg_get_serial_sequence('sec.screens','screen_id'), 25, true);");
            migrationBuilder.Sql("SELECT setval(pg_get_serial_sequence('sec.roles_screens','role_screen_id'), 30, true);");
            migrationBuilder.Sql("SELECT setval(pg_get_serial_sequence('sec.users_roles','user_role_id'), 33, true);");
            migrationBuilder.Sql("SELECT setval(pg_get_serial_sequence('sec.users_applications','user_application_id'), 33, true);");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM sec.users_applications WHERE user_id BETWEEN 3 AND 31;");
            migrationBuilder.Sql("DELETE FROM sec.users_roles WHERE user_id BETWEEN 3 AND 31;");
            migrationBuilder.Sql("DELETE FROM sec.roles_screens WHERE role_id = 3;");
            migrationBuilder.Sql("DELETE FROM sec.users WHERE user_id BETWEEN 3 AND 31;");
            migrationBuilder.DeleteData(schema: "sec", table: "roles", keyColumn: "role_id", keyValue: 3);
        }
    }
}

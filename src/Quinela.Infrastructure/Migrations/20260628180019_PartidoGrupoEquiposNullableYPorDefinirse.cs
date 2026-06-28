using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quinela.Infrastructure.Migrations
{
    /// <summary>
    /// grupo_id, equipo_local_id y equipo_visitante_id pasan a nullable (solo el partido de
    /// grupo los necesita) y se agrega por_definirse. Los partidos de eliminatoria (fase 2)
    /// quedan sin grupo/equipos y marcados como "por definirse" (sus equipos los resuelve el
    /// servicio de distribución según el árbol y las posiciones de grupo).
    /// </summary>
    public partial class PartidoGrupoEquiposNullableYPorDefinirse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "grupo_id",
                table: "partidos",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "equipo_visitante_id",
                table: "partidos",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "equipo_local_id",
                table: "partidos",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<bool>(
                name: "por_definirse",
                table: "partidos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            // La eliminatoria (fase 2) arranca "por definirse": sin grupo ni equipos fijos.
            migrationBuilder.Sql(
                "UPDATE partidos SET grupo_id = NULL, equipo_local_id = NULL, equipo_visitante_id = NULL, por_definirse = TRUE WHERE fase_id = 2;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Antes de volver a NOT NULL hay que rellenar los nulos (placeholders).
            migrationBuilder.Sql(
                "UPDATE partidos SET grupo_id = 1 WHERE grupo_id IS NULL;");
            migrationBuilder.Sql(
                "UPDATE partidos SET equipo_local_id = 1 WHERE equipo_local_id IS NULL;");
            migrationBuilder.Sql(
                "UPDATE partidos SET equipo_visitante_id = 2 WHERE equipo_visitante_id IS NULL;");

            migrationBuilder.DropColumn(
                name: "por_definirse",
                table: "partidos");

            migrationBuilder.AlterColumn<int>(
                name: "grupo_id",
                table: "partidos",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "equipo_visitante_id",
                table: "partidos",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "equipo_local_id",
                table: "partidos",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}

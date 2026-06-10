using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quinela.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniquePrediccionActivaPorPartidoUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_predicciones_partido_id",
                table: "predicciones");

            migrationBuilder.CreateIndex(
                name: "IX_predicciones_partido_id_username",
                table: "predicciones",
                columns: new[] { "partido_id", "username" },
                unique: true,
                filter: "active = true");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_predicciones_partido_id_username",
                table: "predicciones");

            migrationBuilder.CreateIndex(
                name: "IX_predicciones_partido_id",
                table: "predicciones",
                column: "partido_id");
        }
    }
}

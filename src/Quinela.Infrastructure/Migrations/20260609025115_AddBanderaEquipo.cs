using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quinela.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBanderaEquipo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "url_bandera",
                table: "equipos",
                type: "character varying(60)",
                maxLength: 60,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 1,
                column: "url_bandera",
                value: "mx.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 2,
                column: "url_bandera",
                value: "za.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 3,
                column: "url_bandera",
                value: "kr.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 4,
                column: "url_bandera",
                value: "cz.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 5,
                column: "url_bandera",
                value: "ca.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 6,
                column: "url_bandera",
                value: "ba.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 7,
                column: "url_bandera",
                value: "qa.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 8,
                column: "url_bandera",
                value: "ch.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 9,
                column: "url_bandera",
                value: "br.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 10,
                column: "url_bandera",
                value: "ma.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 11,
                column: "url_bandera",
                value: "ht.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 12,
                column: "url_bandera",
                value: "gb-sct.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 13,
                column: "url_bandera",
                value: "us.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 14,
                column: "url_bandera",
                value: "py.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 15,
                column: "url_bandera",
                value: "au.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 16,
                column: "url_bandera",
                value: "tr.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 17,
                column: "url_bandera",
                value: "de.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 18,
                column: "url_bandera",
                value: "cw.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 19,
                column: "url_bandera",
                value: "ci.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 20,
                column: "url_bandera",
                value: "ec.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 21,
                column: "url_bandera",
                value: "nl.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 22,
                column: "url_bandera",
                value: "jp.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 23,
                column: "url_bandera",
                value: "se.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 24,
                column: "url_bandera",
                value: "tn.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 25,
                column: "url_bandera",
                value: "be.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 26,
                column: "url_bandera",
                value: "eg.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 27,
                column: "url_bandera",
                value: "ir.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 28,
                column: "url_bandera",
                value: "nz.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 29,
                column: "url_bandera",
                value: "es.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 30,
                column: "url_bandera",
                value: "cv.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 31,
                column: "url_bandera",
                value: "sa.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 32,
                column: "url_bandera",
                value: "uy.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 33,
                column: "url_bandera",
                value: "fr.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 34,
                column: "url_bandera",
                value: "sn.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 35,
                column: "url_bandera",
                value: "iq.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 36,
                column: "url_bandera",
                value: "no.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 37,
                column: "url_bandera",
                value: "ar.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 38,
                column: "url_bandera",
                value: "dz.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 39,
                column: "url_bandera",
                value: "at.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 40,
                column: "url_bandera",
                value: "jo.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 41,
                column: "url_bandera",
                value: "pt.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 42,
                column: "url_bandera",
                value: "cd.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 43,
                column: "url_bandera",
                value: "uz.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 44,
                column: "url_bandera",
                value: "co.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 45,
                column: "url_bandera",
                value: "gb-eng.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 46,
                column: "url_bandera",
                value: "hr.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 47,
                column: "url_bandera",
                value: "gh.svg");

            migrationBuilder.UpdateData(
                table: "equipos",
                keyColumn: "id",
                keyValue: 48,
                column: "url_bandera",
                value: "pa.svg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "url_bandera",
                table: "equipos");
        }
    }
}

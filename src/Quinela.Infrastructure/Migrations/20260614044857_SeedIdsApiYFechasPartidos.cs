using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quinela.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedIdsApiYFechasPartidos : Migration
    {
        // Pobla el mapeo con el API worldcup26.ir: equipo_id_api / equipo_id_api_largo
        // (48 equipos), partido_id_api y fecha_partido en UTC (72 partidos de grupos).
        // Determinístico: usa los ids del seed (equipos 1-48, partidos 1-72). La fecha
        // viene de local_date del estadio convertida a UTC por su zona horaria.
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
UPDATE equipos SET equipo_id_api='1', equipo_id_api_largo='679c9c6b5749c4077500ea01', updated_at=now(), updated_by='sync-api' WHERE id=1;
UPDATE equipos SET equipo_id_api='2', equipo_id_api_largo='679c9c6b5749c4077500ea02', updated_at=now(), updated_by='sync-api' WHERE id=2;
UPDATE equipos SET equipo_id_api='3', equipo_id_api_largo='679c9c6b5749c4077500ea03', updated_at=now(), updated_by='sync-api' WHERE id=3;
UPDATE equipos SET equipo_id_api='4', equipo_id_api_largo='679c9c6b5749c4077500ea04', updated_at=now(), updated_by='sync-api' WHERE id=4;
UPDATE equipos SET equipo_id_api='5', equipo_id_api_largo='679c9c6b5749c4077500ea05', updated_at=now(), updated_by='sync-api' WHERE id=5;
UPDATE equipos SET equipo_id_api='6', equipo_id_api_largo='679c9c6b5749c4077500ea06', updated_at=now(), updated_by='sync-api' WHERE id=6;
UPDATE equipos SET equipo_id_api='7', equipo_id_api_largo='679c9c6b5749c4077500ea07', updated_at=now(), updated_by='sync-api' WHERE id=7;
UPDATE equipos SET equipo_id_api='8', equipo_id_api_largo='679c9c6b5749c4077500ea08', updated_at=now(), updated_by='sync-api' WHERE id=8;
UPDATE equipos SET equipo_id_api='9', equipo_id_api_largo='679c9c6b5749c4077500ea09', updated_at=now(), updated_by='sync-api' WHERE id=9;
UPDATE equipos SET equipo_id_api='10', equipo_id_api_largo='679c9c6b5749c4077500ea10', updated_at=now(), updated_by='sync-api' WHERE id=10;
UPDATE equipos SET equipo_id_api='11', equipo_id_api_largo='679c9c6b5749c4077500ea11', updated_at=now(), updated_by='sync-api' WHERE id=11;
UPDATE equipos SET equipo_id_api='12', equipo_id_api_largo='679c9c6b5749c4077500ea12', updated_at=now(), updated_by='sync-api' WHERE id=12;
UPDATE equipos SET equipo_id_api='13', equipo_id_api_largo='679c9c6b5749c4077500ea13', updated_at=now(), updated_by='sync-api' WHERE id=13;
UPDATE equipos SET equipo_id_api='14', equipo_id_api_largo='679c9c6b5749c4077500ea14', updated_at=now(), updated_by='sync-api' WHERE id=14;
UPDATE equipos SET equipo_id_api='15', equipo_id_api_largo='679c9c6b5749c4077500ea15', updated_at=now(), updated_by='sync-api' WHERE id=15;
UPDATE equipos SET equipo_id_api='16', equipo_id_api_largo='679c9c6b5749c4077500ea16', updated_at=now(), updated_by='sync-api' WHERE id=16;
UPDATE equipos SET equipo_id_api='17', equipo_id_api_largo='679c9c6b5749c4077500ea17', updated_at=now(), updated_by='sync-api' WHERE id=17;
UPDATE equipos SET equipo_id_api='18', equipo_id_api_largo='679c9c6b5749c4077500ea18', updated_at=now(), updated_by='sync-api' WHERE id=18;
UPDATE equipos SET equipo_id_api='19', equipo_id_api_largo='679c9c6b5749c4077500ea19', updated_at=now(), updated_by='sync-api' WHERE id=19;
UPDATE equipos SET equipo_id_api='20', equipo_id_api_largo='679c9c6b5749c4077500ea20', updated_at=now(), updated_by='sync-api' WHERE id=20;
UPDATE equipos SET equipo_id_api='21', equipo_id_api_largo='679c9c6b5749c4077500ea21', updated_at=now(), updated_by='sync-api' WHERE id=21;
UPDATE equipos SET equipo_id_api='22', equipo_id_api_largo='679c9c6b5749c4077500ea22', updated_at=now(), updated_by='sync-api' WHERE id=22;
UPDATE equipos SET equipo_id_api='23', equipo_id_api_largo='679c9c6b5749c4077500ea23', updated_at=now(), updated_by='sync-api' WHERE id=23;
UPDATE equipos SET equipo_id_api='24', equipo_id_api_largo='679c9c6b5749c4077500ea24', updated_at=now(), updated_by='sync-api' WHERE id=24;
UPDATE equipos SET equipo_id_api='25', equipo_id_api_largo='679c9c6b5749c4077500ea25', updated_at=now(), updated_by='sync-api' WHERE id=25;
UPDATE equipos SET equipo_id_api='26', equipo_id_api_largo='679c9c6b5749c4077500ea26', updated_at=now(), updated_by='sync-api' WHERE id=26;
UPDATE equipos SET equipo_id_api='27', equipo_id_api_largo='679c9c6b5749c4077500ea27', updated_at=now(), updated_by='sync-api' WHERE id=27;
UPDATE equipos SET equipo_id_api='28', equipo_id_api_largo='679c9c6b5749c4077500ea28', updated_at=now(), updated_by='sync-api' WHERE id=28;
UPDATE equipos SET equipo_id_api='29', equipo_id_api_largo='679c9c6b5749c4077500ea29', updated_at=now(), updated_by='sync-api' WHERE id=29;
UPDATE equipos SET equipo_id_api='30', equipo_id_api_largo='679c9c6b5749c4077500ea30', updated_at=now(), updated_by='sync-api' WHERE id=30;
UPDATE equipos SET equipo_id_api='31', equipo_id_api_largo='679c9c6b5749c4077500ea31', updated_at=now(), updated_by='sync-api' WHERE id=31;
UPDATE equipos SET equipo_id_api='32', equipo_id_api_largo='679c9c6b5749c4077500ea32', updated_at=now(), updated_by='sync-api' WHERE id=32;
UPDATE equipos SET equipo_id_api='33', equipo_id_api_largo='679c9c6b5749c4077500ea33', updated_at=now(), updated_by='sync-api' WHERE id=33;
UPDATE equipos SET equipo_id_api='34', equipo_id_api_largo='679c9c6b5749c4077500ea34', updated_at=now(), updated_by='sync-api' WHERE id=34;
UPDATE equipos SET equipo_id_api='35', equipo_id_api_largo='679c9c6b5749c4077500ea35', updated_at=now(), updated_by='sync-api' WHERE id=35;
UPDATE equipos SET equipo_id_api='36', equipo_id_api_largo='679c9c6b5749c4077500ea36', updated_at=now(), updated_by='sync-api' WHERE id=36;
UPDATE equipos SET equipo_id_api='37', equipo_id_api_largo='679c9c6b5749c4077500ea37', updated_at=now(), updated_by='sync-api' WHERE id=37;
UPDATE equipos SET equipo_id_api='38', equipo_id_api_largo='679c9c6b5749c4077500ea38', updated_at=now(), updated_by='sync-api' WHERE id=38;
UPDATE equipos SET equipo_id_api='39', equipo_id_api_largo='679c9c6b5749c4077500ea39', updated_at=now(), updated_by='sync-api' WHERE id=39;
UPDATE equipos SET equipo_id_api='40', equipo_id_api_largo='679c9c6b5749c4077500ea40', updated_at=now(), updated_by='sync-api' WHERE id=40;
UPDATE equipos SET equipo_id_api='41', equipo_id_api_largo='679c9c6b5749c4077500ea41', updated_at=now(), updated_by='sync-api' WHERE id=41;
UPDATE equipos SET equipo_id_api='42', equipo_id_api_largo='679c9c6b5749c4077500ea42', updated_at=now(), updated_by='sync-api' WHERE id=42;
UPDATE equipos SET equipo_id_api='43', equipo_id_api_largo='679c9c6b5749c4077500ea43', updated_at=now(), updated_by='sync-api' WHERE id=43;
UPDATE equipos SET equipo_id_api='44', equipo_id_api_largo='679c9c6b5749c4077500ea44', updated_at=now(), updated_by='sync-api' WHERE id=44;
UPDATE equipos SET equipo_id_api='45', equipo_id_api_largo='679c9c6b5749c4077500ea45', updated_at=now(), updated_by='sync-api' WHERE id=45;
UPDATE equipos SET equipo_id_api='46', equipo_id_api_largo='679c9c6b5749c4077500ea46', updated_at=now(), updated_by='sync-api' WHERE id=46;
UPDATE equipos SET equipo_id_api='47', equipo_id_api_largo='679c9c6b5749c4077500ea47', updated_at=now(), updated_by='sync-api' WHERE id=47;
UPDATE equipos SET equipo_id_api='48', equipo_id_api_largo='679c9c6b5749c4077500ea48', updated_at=now(), updated_by='sync-api' WHERE id=48;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e001', fecha_partido='2026-06-11 19:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=1;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e002', fecha_partido='2026-06-12 02:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=2;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e003', fecha_partido='2026-06-12 19:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=3;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e004', fecha_partido='2026-06-13 01:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=4;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e005', fecha_partido='2026-06-14 01:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=7;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e006', fecha_partido='2026-06-14 04:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=8;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e007', fecha_partido='2026-06-13 22:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=6;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e008', fecha_partido='2026-06-13 19:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=5;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e009', fecha_partido='2026-06-14 23:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=11;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e010', fecha_partido='2026-06-14 17:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=9;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e011', fecha_partido='2026-06-14 20:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=10;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e012', fecha_partido='2026-06-15 02:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=12;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e013', fecha_partido='2026-06-16 01:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=16;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e014', fecha_partido='2026-06-15 16:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=13;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e015', fecha_partido='2026-06-15 19:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=14;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e016', fecha_partido='2026-06-15 22:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=15;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e017', fecha_partido='2026-06-16 19:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=17;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e018', fecha_partido='2026-06-16 22:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=18;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e019', fecha_partido='2026-06-17 01:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=19;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e020', fecha_partido='2026-06-17 04:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=20;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e021', fecha_partido='2026-06-17 17:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=21;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e022', fecha_partido='2026-06-17 20:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=22;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e023', fecha_partido='2026-06-18 02:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=24;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e024', fecha_partido='2026-06-17 23:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=23;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e025', fecha_partido='2026-06-19 01:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=28;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e026', fecha_partido='2026-06-18 19:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=26;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e027', fecha_partido='2026-06-18 22:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=27;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e028', fecha_partido='2026-06-18 16:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=25;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e029', fecha_partido='2026-06-20 01:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=31;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e030', fecha_partido='2026-06-19 22:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=30;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e031', fecha_partido='2026-06-19 19:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=29;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e032', fecha_partido='2026-06-20 03:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=32;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e033', fecha_partido='2026-06-20 20:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=34;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e034', fecha_partido='2026-06-21 00:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=35;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e035', fecha_partido='2026-06-20 17:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=33;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e036', fecha_partido='2026-06-21 04:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=36;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e037', fecha_partido='2026-06-21 19:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=38;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e038', fecha_partido='2026-06-22 01:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=40;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e039', fecha_partido='2026-06-21 16:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=37;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e040', fecha_partido='2026-06-21 22:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=39;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e041', fecha_partido='2026-06-22 21:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=42;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e042', fecha_partido='2026-06-23 00:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=43;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e043', fecha_partido='2026-06-22 17:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=41;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e044', fecha_partido='2026-06-23 03:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=44;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e045', fecha_partido='2026-06-23 17:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=45;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e046', fecha_partido='2026-06-23 23:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=47;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e047', fecha_partido='2026-06-24 02:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=48;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e048', fecha_partido='2026-06-23 20:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=46;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e049', fecha_partido='2026-06-24 22:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=52;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e050', fecha_partido='2026-06-24 22:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=51;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e051', fecha_partido='2026-06-25 01:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=53;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e052', fecha_partido='2026-06-25 01:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=54;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e053', fecha_partido='2026-06-24 19:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=50;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e054', fecha_partido='2026-06-24 19:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=49;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e055', fecha_partido='2026-06-25 20:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=55;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e056', fecha_partido='2026-06-25 20:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=56;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e057', fecha_partido='2026-06-26 02:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=60;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e058', fecha_partido='2026-06-26 02:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=59;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e059', fecha_partido='2026-06-25 23:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=58;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e060', fecha_partido='2026-06-25 23:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=57;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e061', fecha_partido='2026-06-26 19:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=62;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e062', fecha_partido='2026-06-26 19:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=61;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e063', fecha_partido='2026-06-27 03:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=66;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e064', fecha_partido='2026-06-27 03:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=65;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e065', fecha_partido='2026-06-27 00:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=63;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e066', fecha_partido='2026-06-27 00:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=64;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e067', fecha_partido='2026-06-27 21:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=67;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e068', fecha_partido='2026-06-27 21:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=68;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e069', fecha_partido='2026-06-28 02:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=71;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e070', fecha_partido='2026-06-28 02:00:00+00', updated_at=now(), updated_by='sync-api' WHERE id=72;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e071', fecha_partido='2026-06-27 23:30:00+00', updated_at=now(), updated_by='sync-api' WHERE id=69;
UPDATE partidos SET partido_id_api='679c9c8a5749c4077500e072', fecha_partido='2026-06-27 23:30:00+00', updated_at=now(), updated_by='sync-api' WHERE id=70;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revierte solo los ids del API (la fecha placeholder original no se restaura).
            migrationBuilder.Sql(@"
UPDATE equipos SET equipo_id_api = NULL, equipo_id_api_largo = NULL WHERE equipo_id_api IS NOT NULL;
UPDATE partidos SET partido_id_api = NULL WHERE partido_id_api IS NOT NULL;
");
        }
    }
}

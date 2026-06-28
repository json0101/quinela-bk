using System.Reflection;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Quinela.Application.Common.Abstractions;
using Quinela.Domain.Entities;

namespace Quinela.Application.Features.Eliminatoria
{
    // ----- DTOs del preview -----
    public sealed record BracketPreviewDto(int TorneoId, List<BracketRondaDto> Rondas);
    public sealed record BracketRondaDto(string Ronda, List<BracketPartidoDto> Partidos);
    public sealed record BracketPartidoDto(
        int Id, DateTime Fecha, string Ronda, string Local, string Visitante, string Estado,
        int? GolesLocal, int? GolesVisitante, int? PenalesLocal, int? PenalesVisitante, string? Ganador,
        int? FuenteLocalId, int? FuenteVisitanteId,
        string? LocalBandera, string? VisitanteBandera, bool PorDefinirse);

    /// <summary>
    /// Arma la eliminatoria del Mundial 2026 a partir de las posiciones de grupo (ya calculadas
    /// en GrupoEquipo) y la estructura oficial de la FIFA (BracketMundial2026.json):
    /// resuelve los equipos de los dieciseisavos (1ro/2do/mejor 3ro), propaga los ganadores por
    /// el árbol (campos partido_ganador_local_id / partido_ganador_visitante_id) y calcula el
    /// ganador de cada partido terminado (por goles, o por penales si quedó empatado).
    /// </summary>
    public interface IDistributionEliminatoryWorldCup2026
    {
        /// <summary>Devuelve el cuadro resuelto (sin persistir).</summary>
        Task<BracketPreviewDto> PreviewAsync(int torneoId, CancellationToken ct = default);

        /// <summary>Resuelve y PERSISTE: materializa equipos, ganadores y propaga el árbol.</summary>
        Task<BracketPreviewDto> RecalcularAsync(int torneoId, CancellationToken ct = default);
    }

    public sealed class DistributionEliminatoryWorldCup2026 : IDistributionEliminatoryWorldCup2026
    {
        private readonly IRepository<Partido> _partidos;
        private readonly IRepository<GrupoEquipo> _gruposEquipos;
        private readonly IRepository<Equipo> _equipos;
        private readonly IUnitOfWork _uow;

        public DistributionEliminatoryWorldCup2026(IRepository<Partido> partidos,
            IRepository<GrupoEquipo> gruposEquipos, IRepository<Equipo> equipos, IUnitOfWork uow)
        { _partidos = partidos; _gruposEquipos = gruposEquipos; _equipos = equipos; _uow = uow; }

        public async Task<BracketPreviewDto> PreviewAsync(int torneoId, CancellationToken ct = default)
        {
            var r = await ResolverAsync(torneoId, ct);
            return Construir(torneoId, r);
        }

        public async Task<BracketPreviewDto> RecalcularAsync(int torneoId, CancellationToken ct = default)
        {
            var r = await ResolverAsync(torneoId, ct);

            var ahora = DateTime.UtcNow;
            foreach (var def in Bracket.Partidos)
            {
                if (!r.PorId.TryGetValue(def.Id, out var p)) continue;
                var local = r.Local.GetValueOrDefault(def.Id);
                var visit = r.Visitante.GetValueOrDefault(def.Id);
                var ganador = r.Ganador.GetValueOrDefault(def.Id);

                var cambio = false;
                // Slot alimentado por el árbol (PartidoGanador*Id): refleja EXACTO el ganador
                // de la fuente, incluso null (si la fuente no terminó o se eliminó -> se limpia).
                // Slot de grupo (dieciseisavos, sin fuente): solo se setea cuando resuelve.
                if (p.PartidoGanadorLocalId.HasValue)
                {
                    if (p.EquipoLocalId != local) { p.EquipoLocalId = local; cambio = true; }
                }
                else if (local is int l && p.EquipoLocalId != l) { p.EquipoLocalId = l; cambio = true; }

                if (p.PartidoGanadorVisitanteId.HasValue)
                {
                    if (p.EquipoVisitanteId != visit) { p.EquipoVisitanteId = visit; cambio = true; }
                }
                else if (visit is int v && p.EquipoVisitanteId != v) { p.EquipoVisitanteId = v; cambio = true; }

                if (p.EquipoGanadorId != ganador) { p.EquipoGanadorId = ganador; cambio = true; }
                if (cambio) { p.UpdatedAt = ahora; p.UpdatedBy = "eliminatoria"; }
            }
            await _uow.SaveChangesAsync(ct);

            return Construir(torneoId, r);
        }

        // ----- Resolución (sin persistir) -----
        private async Task<Resuelto> ResolverAsync(int torneoId, CancellationToken ct)
        {
            var ids = Bracket.Partidos.Select(x => x.Id).ToList();

            // Solo partidos ACTIVOS de la BD: un partido eliminado (Active=false) no se carga,
            // así no aparece en el cuadro y su dependiente queda sin ese equipo.
            var partidos = await _partidos.GetDbSet()
                .Where(p => p.TorneoId == torneoId && p.Active && ids.Contains(p.Id))
                .ToListAsync(ct);
            var porId = partidos.ToDictionary(p => p.Id);

            // Tabla de posiciones de los grupos (Posicion ya la calcula RankingService).
            var standings = await _gruposEquipos.GetDbSet().AsNoTracking()
                .Where(ge => ge.TorneoId == torneoId)
                .Select(ge => new StandingRow(ge.Grupo.Nombre, ge.EquipoId, ge.Posicion, ge.Pts, ge.Diff, ge.GF))
                .ToListAsync(ct);

            var equipos = await _equipos.GetDbSet().AsNoTracking()
                .Where(e => e.TorneoId == torneoId)
                .Select(e => new { e.Id, e.Nombre, e.UrlBandera })
                .ToListAsync(ct);
            var nombres = equipos.ToDictionary(e => e.Id, e => e.Nombre);
            var banderas = equipos.ToDictionary(e => e.Id, e => e.UrlBandera);

            var pos1 = standings.Where(s => s.Posicion == 1).ToDictionary(s => s.Grupo, s => s.EquipoId);
            var pos2 = standings.Where(s => s.Posicion == 2).ToDictionary(s => s.Grupo, s => s.EquipoId);

            // Mejores terceros: top 8 por Pts -> Diff -> GF -> grupo. PREVIEW: simplificación
            // respecto a la tabla FIFA de 495 combinaciones (se corrige luego).
            var mejoresTerceros = standings.Where(s => s.Posicion == 3)
                .OrderByDescending(s => s.Pts).ThenByDescending(s => s.Diff)
                .ThenByDescending(s => s.GF).ThenBy(s => s.Grupo)
                .Take(8).Select(s => s.EquipoId).ToList();

            // Asigna cada tercero a los slots "3ro" en orden de aparición (matches 74,77,79,...).
            var terceroPorPartido = new Dictionary<int, int>();
            var iTercero = 0;
            foreach (var def in Bracket.Partidos)
            {
                if (def.Local == Tercero || def.Visitante == Tercero)
                {
                    if (iTercero < mejoresTerceros.Count) terceroPorPartido[def.Id] = mejoresTerceros[iTercero];
                    iTercero++;
                }
            }

            var local = new Dictionary<int, int?>();
            var visitante = new Dictionary<int, int?>();
            var ganador = new Dictionary<int, int?>();
            var perdedor = new Dictionary<int, int?>();

            // Orden ascendente: los partidos que alimentan (ids menores) se resuelven primero.
            foreach (var def in Bracket.Partidos.OrderBy(x => x.Id))
            {
                porId.TryGetValue(def.Id, out var p);

                int? l = p?.PartidoGanadorLocalId is int pl
                    ? ganador.GetValueOrDefault(pl)
                    : ResolverSlot(def.Local, def.Id, pos1, pos2, terceroPorPartido, ganador, perdedor);
                int? v = p?.PartidoGanadorVisitanteId is int pv
                    ? ganador.GetValueOrDefault(pv)
                    : ResolverSlot(def.Visitante, def.Id, pos1, pos2, terceroPorPartido, ganador, perdedor);

                local[def.Id] = l;
                visitante[def.Id] = v;

                var (g, lo) = CalcularGanador(p, l, v);
                ganador[def.Id] = g;
                perdedor[def.Id] = lo;
            }

            return new Resuelto(porId, nombres, banderas, local, visitante, ganador);
        }

        private static int? ResolverSlot(string code, int matchId,
            Dictionary<string, int> pos1, Dictionary<string, int> pos2,
            Dictionary<int, int> terceroPorPartido,
            Dictionary<int, int?> ganador, Dictionary<int, int?> perdedor)
        {
            if (code == Tercero)
                return terceroPorPartido.TryGetValue(matchId, out var t) ? t : (int?)null;
            if (code.Length >= 2 && code[0] == '1')
                return pos1.TryGetValue(code[1..], out var a) ? a : (int?)null;
            if (code.Length >= 2 && code[0] == '2')
                return pos2.TryGetValue(code[1..], out var b) ? b : (int?)null;
            if (code.Length >= 2 && code[0] == 'W' && int.TryParse(code[1..], out var w))
                return ganador.GetValueOrDefault(w);
            if (code.Length >= 2 && code[0] == 'L' && int.TryParse(code[1..], out var lp))
                return perdedor.GetValueOrDefault(lp);
            return null;
        }

        // Ganador/perdedor de un partido terminado: por goles; si empató, por penales.
        private static (int? ganador, int? perdedor) CalcularGanador(Partido? p, int? localId, int? visitId)
        {
            // Un partido eliminado (Active=false) o no terminado no aporta ganador: así, al
            // borrar el partido de referencia, el dependiente que lo usaba queda en null.
            if (p is null || !p.Active || p.Estado != 'T' || localId is null || visitId is null) return (null, null);

            int lado; // +1 gana local, -1 gana visitante, 0 sin definir
            if (p.ResultadoLocalId is int rl && p.ResultadoVisitanteId is int rv && rl != rv)
                lado = rl > rv ? 1 : -1;
            else if (p.PenalesAnotadosLocal is int pl && p.PenalesAnotadosVisitante is int pv && pl != pv)
                lado = pl > pv ? 1 : -1;
            else
                return (null, null);

            return lado == 1 ? (localId, visitId) : (visitId, localId);
        }

        // ----- Construcción del DTO -----
        private BracketPreviewDto Construir(int torneoId, Resuelto r)
        {
            var rondas = Bracket.Partidos
                // Solo se muestran los partidos que existen activos en la BD (el JSON solo
                // aporta la estructura del árbol, no obliga a mostrar choques inexistentes).
                .Where(def => r.PorId.ContainsKey(def.Id))
                .GroupBy(x => x.Ronda)
                .Select(g => new BracketRondaDto(g.Key, g.Select(def =>
                {
                    r.PorId.TryGetValue(def.Id, out var p);
                    var localId = r.Local.GetValueOrDefault(def.Id);
                    var visitId = r.Visitante.GetValueOrDefault(def.Id);
                    return new BracketPartidoDto(
                        def.Id, def.Fecha, def.Ronda,
                        Etiqueta(def.Local, localId, r.Nombres),
                        Etiqueta(def.Visitante, visitId, r.Nombres),
                        (p?.Estado ?? 'P').ToString(),
                        p?.ResultadoLocalId, p?.ResultadoVisitanteId,
                        p?.PenalesAnotadosLocal, p?.PenalesAnotadosVisitante,
                        r.Ganador.GetValueOrDefault(def.Id) is int gid ? r.Nombres.GetValueOrDefault(gid) : null,
                        FuenteId(def.Local), FuenteId(def.Visitante),
                        localId is int li ? r.Banderas.GetValueOrDefault(li) : null,
                        visitId is int vi ? r.Banderas.GetValueOrDefault(vi) : null,
                        localId is null || visitId is null);
                }).ToList()))
                .ToList();

            return new BracketPreviewDto(torneoId, rondas);
        }

        // Id del partido que alimenta un slot W{n}/L{n} (null si es un slot de grupo).
        private static int? FuenteId(string code) =>
            code.Length >= 2 && (code[0] == 'W' || code[0] == 'L') && int.TryParse(code[1..], out var n) ? n : (int?)null;

        // Nombre del equipo si está resuelto; si no, la etiqueta del slot (1° Grupo A, Ganador #74...).
        private static string Etiqueta(string code, int? equipoId, Dictionary<int, string> nombres) =>
            equipoId is int id && nombres.TryGetValue(id, out var n) ? n : SlotLabel(code);

        private static string SlotLabel(string code)
        {
            if (code == Tercero) return "Mejor 3°";
            if (code.Length >= 2 && code[0] == '1') return $"1° Grupo {code[1..]}";
            if (code.Length >= 2 && code[0] == '2') return $"2° Grupo {code[1..]}";
            if (code.Length >= 2 && code[0] == 'W') return $"Ganador #{code[1..]}";
            if (code.Length >= 2 && code[0] == 'L') return $"Perdedor #{code[1..]}";
            return code;
        }

        // ----- Estructura del bracket (recurso embebido, se carga una vez) -----
        private const string Tercero = "3ro";

        private static readonly BracketDef Bracket = CargarBracket();

        private static BracketDef CargarBracket()
        {
            var asm = typeof(DistributionEliminatoryWorldCup2026).Assembly;
            var name = asm.GetManifestResourceNames()
                .Single(n => n.EndsWith("BracketMundial2026.json", StringComparison.Ordinal));
            using var stream = asm.GetManifestResourceStream(name)!;
            var def = JsonSerializer.Deserialize<BracketDef>(stream,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return def!;
        }

        private sealed record Resuelto(
            Dictionary<int, Partido> PorId,
            Dictionary<int, string> Nombres,
            Dictionary<int, string?> Banderas,
            Dictionary<int, int?> Local,
            Dictionary<int, int?> Visitante,
            Dictionary<int, int?> Ganador);

        private sealed record StandingRow(string Grupo, int EquipoId, int Posicion, int Pts, int Diff, int GF);

        private sealed record BracketDef(int TorneoId, string Descripcion, List<BracketPartidoDef> Partidos);
        private sealed record BracketPartidoDef(int Id, string Ronda, DateTime Fecha, string Local, string Visitante);
    }
}

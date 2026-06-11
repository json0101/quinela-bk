using Microsoft.EntityFrameworkCore;
using Quinela.Application.Common.Abstractions;
using Quinela.Domain.Entities;
using RankingEntity = Quinela.Domain.Entities.Ranking;

namespace Quinela.Application.Features.Ranking
{
    /// <summary>
    /// Recalcula, para un torneo: la tabla de posiciones de sus grupos y el ranking de cada
    /// una de sus quinielas, según la parametrización de cada tipo de partido.
    /// Es idempotente (parte de cero y vuelve a sumar).
    /// </summary>
    public sealed class RankingService : IRankingService
    {
        private readonly IRepository<Partido> _partidos;
        private readonly IRepository<Prediccion> _predicciones;
        private readonly IRepository<GrupoEquipo> _gruposEquipos;
        private readonly IRepository<RankingEntity> _ranking;
        private readonly IRepository<Quiniela> _quinielas;
        private readonly IUnitOfWork _uow;

        public RankingService(
            IRepository<Partido> partidos,
            IRepository<Prediccion> predicciones,
            IRepository<GrupoEquipo> gruposEquipos,
            IRepository<RankingEntity> ranking,
            IRepository<Quiniela> quinielas,
            IUnitOfWork uow)
        {
            _partidos = partidos;
            _predicciones = predicciones;
            _gruposEquipos = gruposEquipos;
            _ranking = ranking;
            _quinielas = quinielas;
            _uow = uow;
        }

        public async Task RecalcularAsync(int torneoId, CancellationToken ct = default)
        {
            var ahora = DateTime.UtcNow;

            // Partidos jugados (en curso o terminados) con marcador, del torneo.
            var jugados = await _partidos.GetDbSet().AsNoTracking()
                .Where(p => p.Active && p.TorneoId == torneoId
                    && (p.Estado == 'E' || p.Estado == 'T')
                    && p.ResultadoLocalId != null && p.ResultadoVisitanteId != null)
                .Select(p => new PartidoJugado
                {
                    Id = p.Id,
                    GrupoId = p.GrupoId,
                    EquipoLocalId = p.EquipoLocalId,
                    EquipoVisitanteId = p.EquipoVisitanteId,
                    ResultadoLocal = p.ResultadoLocalId!.Value,
                    ResultadoVisitante = p.ResultadoVisitanteId!.Value,
                    PtsVictoria = p.TipoPartido!.PtsPartidoVictoria,
                    PtsEmpate = p.TipoPartido.PtsPartidoEmpate,
                    PtsExacto = p.TipoPartido.PtsQuinelaResultadoExacto,
                    PtsAcertado = p.TipoPartido.PtsQuinelaResultadoAcertado,
                })
                .ToListAsync(ct);

            await RecalcularGruposAsync(torneoId, jugados, ahora, ct);
            await RecalcularRankingsAsync(torneoId, jugados, ahora, ct);

            await _uow.SaveChangesAsync(ct);
        }

        // --- Tabla de posiciones de los grupos del torneo ---
        private async Task RecalcularGruposAsync(int torneoId, List<PartidoJugado> jugados, DateTime ahora, CancellationToken ct)
        {
            var acc = new Dictionary<(int grupo, int equipo), (int gf, int gc, int pts)>();
            void Sumar(int grupo, int equipo, int gf, int gc, int pts)
            {
                var cur = acc.GetValueOrDefault((grupo, equipo));
                acc[(grupo, equipo)] = (cur.gf + gf, cur.gc + gc, cur.pts + pts);
            }

            foreach (var j in jugados)
            {
                var signo = Math.Sign(j.ResultadoLocal - j.ResultadoVisitante);
                var ptsLocal = signo > 0 ? j.PtsVictoria : signo == 0 ? j.PtsEmpate : 0;
                var ptsVisitante = signo < 0 ? j.PtsVictoria : signo == 0 ? j.PtsEmpate : 0;
                Sumar(j.GrupoId, j.EquipoLocalId, j.ResultadoLocal, j.ResultadoVisitante, ptsLocal);
                Sumar(j.GrupoId, j.EquipoVisitanteId, j.ResultadoVisitante, j.ResultadoLocal, ptsVisitante);
            }

            var filas = await _gruposEquipos.GetDbSet().Where(x => x.TorneoId == torneoId).ToListAsync(ct);
            foreach (var ge in filas)
            {
                var a = acc.GetValueOrDefault((ge.GrupoId, ge.EquipoId));
                ge.GF = a.gf;
                ge.GC = a.gc;
                ge.Diff = a.gf - a.gc;
                ge.Pts = a.pts;
                ge.UpdatedAt = ahora;
                ge.UpdatedBy = "ranking";
            }

            foreach (var grupo in filas.GroupBy(x => x.GrupoId))
            {
                var ordenado = grupo
                    .OrderByDescending(x => x.Pts)
                    .ThenByDescending(x => x.Diff)
                    .ThenByDescending(x => x.GF)
                    .ToList();
                for (var i = 0; i < ordenado.Count; i++)
                    ordenado[i].Posicion = i + 1;
            }
        }

        // --- Ranking de cada quiniela del torneo ---
        private async Task RecalcularRankingsAsync(int torneoId, List<PartidoJugado> jugados, DateTime ahora, CancellationToken ct)
        {
            var jugadosPorId = jugados.ToDictionary(x => x.Id);

            var quinielaIds = await _quinielas.GetDbSet().AsNoTracking()
                .Where(q => q.TorneoId == torneoId)
                .Select(q => q.Id)
                .ToListAsync(ct);
            if (quinielaIds.Count == 0) return;

            var preds = await _predicciones.GetDbSet().AsNoTracking()
                .Where(x => x.Active && quinielaIds.Contains(x.QuinielaId)
                    && x.Team1Resultado != null && x.Team2Resultado != null)
                .Select(x => new { x.QuinielaId, x.Username, x.PartidoId, T1 = x.Team1Resultado!.Value, T2 = x.Team2Resultado!.Value })
                .ToListAsync(ct);

            // Acumulado por (quiniela, usuario).
            var agg = new Dictionary<(int quiniela, string usuario), (int pts, int exacto, int atinado)>();
            foreach (var pr in preds)
            {
                if (!jugadosPorId.TryGetValue(pr.PartidoId, out var j)) continue;

                var exacto = pr.T1 == j.ResultadoLocal && pr.T2 == j.ResultadoVisitante;
                var acertado = Math.Sign(pr.T1 - pr.T2) == Math.Sign(j.ResultadoLocal - j.ResultadoVisitante);
                var pts = exacto ? j.PtsExacto : acertado ? j.PtsAcertado : 0;

                var key = (pr.QuinielaId, pr.Username);
                var cur = agg.GetValueOrDefault(key);
                agg[key] = (cur.pts + pts, cur.exacto + (exacto ? 1 : 0), cur.atinado + (!exacto && acertado ? 1 : 0));
            }

            var filas = await _ranking.GetDbSet()
                .Where(r => quinielaIds.Contains(r.QuinielaId)).ToListAsync(ct);
            var porClave = filas.ToDictionary(r => (r.QuinielaId, r.Usuario));

            foreach (var kv in agg)
            {
                if (porClave.TryGetValue(kv.Key, out var r))
                {
                    r.Pts = kv.Value.pts;
                    r.ResultadoExacto = kv.Value.exacto;
                    r.ResultadoAtinado = kv.Value.atinado;
                    r.UpdatedAt = ahora;
                    r.UpdatedBy = "ranking";
                }
                else
                {
                    _ranking.Insert(new RankingEntity
                    {
                        QuinielaId = kv.Key.quiniela,
                        Usuario = kv.Key.usuario,
                        Pts = kv.Value.pts,
                        ResultadoExacto = kv.Value.exacto,
                        ResultadoAtinado = kv.Value.atinado,
                        Active = true,
                        CreatedAt = ahora,
                        CreatedBy = "ranking",
                    });
                }
            }

            // Filas existentes sin predicciones contadas -> quedan en cero.
            foreach (var r in filas.Where(r => !agg.ContainsKey((r.QuinielaId, r.Usuario))))
            {
                r.Pts = 0;
                r.ResultadoExacto = 0;
                r.ResultadoAtinado = 0;
                r.UpdatedAt = ahora;
                r.UpdatedBy = "ranking";
            }
        }

        private sealed class PartidoJugado
        {
            public int Id { get; set; }
            public int GrupoId { get; set; }
            public int EquipoLocalId { get; set; }
            public int EquipoVisitanteId { get; set; }
            public int ResultadoLocal { get; set; }
            public int ResultadoVisitante { get; set; }
            public int PtsVictoria { get; set; }
            public int PtsEmpate { get; set; }
            public int PtsExacto { get; set; }
            public int PtsAcertado { get; set; }
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Quinela.Application.Common.Abstractions;
using Quinela.Domain.Entities;
using RankingEntity = Quinela.Domain.Entities.Ranking;

namespace Quinela.Application.Features.Ranking
{
    /// <summary>
    /// Recalcula posiciones de grupos y ranking de la quiniela según la parametrización
    /// de cada tipo de partido. Se recalcula completo (idempotente).
    /// </summary>
    public sealed class RankingService : IRankingService
    {
        private readonly IRepository<Partido> _partidos;
        private readonly IRepository<Prediccion> _predicciones;
        private readonly IRepository<GrupoEquipo> _gruposEquipos;
        private readonly IRepository<RankingEntity> _ranking;
        private readonly IUnitOfWork _uow;

        public RankingService(
            IRepository<Partido> partidos,
            IRepository<Prediccion> predicciones,
            IRepository<GrupoEquipo> gruposEquipos,
            IRepository<RankingEntity> ranking,
            IUnitOfWork uow)
        {
            _partidos = partidos;
            _predicciones = predicciones;
            _gruposEquipos = gruposEquipos;
            _ranking = ranking;
            _uow = uow;
        }

        public async Task RecalcularAsync(CancellationToken ct = default)
        {
            var ahora = DateTime.UtcNow;

            // Partidos jugados (en curso o terminados) con marcador, con la parametrización del tipo.
            var jugados = await _partidos.GetDbSet().AsNoTracking()
                .Where(p => p.Active
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

            await RecalcularGruposAsync(jugados, ahora, ct);
            await RecalcularQuinielaAsync(jugados, ahora, ct);

            await _uow.SaveChangesAsync(ct);
        }

        // --- Tabla de posiciones de los grupos (puntos del partido: victoria/empate) ---
        private async Task RecalcularGruposAsync(List<PartidoJugado> jugados, DateTime ahora, CancellationToken ct)
        {
            // Acumulado por (grupo, equipo): goles a favor, en contra y puntos.
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

            var filas = await _gruposEquipos.GetDbSet().ToListAsync(ct);
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

            // Posición dentro de cada grupo: más puntos, luego diferencia, luego goles a favor.
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

        // --- Ranking de la quiniela (puntos exacto/acertado por predicción) ---
        private async Task RecalcularQuinielaAsync(List<PartidoJugado> jugados, DateTime ahora, CancellationToken ct)
        {
            var jugadosPorId = jugados.ToDictionary(x => x.Id);

            var preds = await _predicciones.GetDbSet().AsNoTracking()
                .Where(x => x.Active && x.Team1Resultado != null && x.Team2Resultado != null)
                .Select(x => new { x.Username, x.PartidoId, T1 = x.Team1Resultado!.Value, T2 = x.Team2Resultado!.Value })
                .ToListAsync(ct);

            var agg = new Dictionary<string, (int pts, int exacto, int atinado)>();
            foreach (var pr in preds)
            {
                if (!jugadosPorId.TryGetValue(pr.PartidoId, out var j)) continue;

                var exacto = pr.T1 == j.ResultadoLocal && pr.T2 == j.ResultadoVisitante;
                var acertado = Math.Sign(pr.T1 - pr.T2) == Math.Sign(j.ResultadoLocal - j.ResultadoVisitante);

                var pts = exacto ? j.PtsExacto : acertado ? j.PtsAcertado : 0;
                var cur = agg.GetValueOrDefault(pr.Username);
                agg[pr.Username] = (
                    cur.pts + pts,
                    cur.exacto + (exacto ? 1 : 0),
                    cur.atinado + (!exacto && acertado ? 1 : 0));
            }

            var filas = await _ranking.GetDbSet().ToListAsync(ct);
            var porUsuario = filas.ToDictionary(r => r.Usuario);

            foreach (var kv in agg)
            {
                if (porUsuario.TryGetValue(kv.Key, out var r))
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
                        Usuario = kv.Key,
                        Pts = kv.Value.pts,
                        ResultadoExacto = kv.Value.exacto,
                        ResultadoAtinado = kv.Value.atinado,
                        Active = true,
                        CreatedAt = ahora,
                        CreatedBy = "ranking",
                    });
                }
            }

            // Usuarios con fila de ranking pero sin predicciones contadas -> quedan en cero.
            foreach (var r in filas.Where(r => !agg.ContainsKey(r.Usuario)))
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

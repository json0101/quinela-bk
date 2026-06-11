using MediatR;
using Microsoft.EntityFrameworkCore;
using Quinela.Application.Common.Abstractions;
using Quinela.Application.Common.Results;
using Quinela.Domain.Entities;

namespace Quinela.Application.Features.Partidos
{
    // Calendario de una quiniela (partidos de su torneo) + filtro opcional por rango de fecha.
    public sealed record GetPartidosCalendarioQuery(int QuinielaId, DateTime? Desde, DateTime? Hasta)
        : IRequest<Result<List<PartidoCalendarioDto>>>;

    internal sealed class GetPartidosCalendarioHandler
        : IRequestHandler<GetPartidosCalendarioQuery, Result<List<PartidoCalendarioDto>>>
    {
        private readonly IRepository<Partido> _partidos;
        private readonly IRepository<Prediccion> _predicciones;
        private readonly IRepository<Quiniela> _quinielas;
        private readonly ICurrentUser _currentUser;

        public GetPartidosCalendarioHandler(
            IRepository<Partido> partidos, IRepository<Prediccion> predicciones,
            IRepository<Quiniela> quinielas, ICurrentUser currentUser)
        { _partidos = partidos; _predicciones = predicciones; _quinielas = quinielas; _currentUser = currentUser; }

        public async Task<Result<List<PartidoCalendarioDto>>> Handle(GetPartidosCalendarioQuery request, CancellationToken ct)
        {
            // La quiniela determina el torneo cuyos partidos se muestran.
            var torneoId = await _quinielas.GetDbSet().AsNoTracking()
                .Where(q => q.Id == request.QuinielaId).Select(q => (int?)q.TorneoId).FirstOrDefaultAsync(ct);
            if (torneoId is null) return Result.Success(new List<PartidoCalendarioDto>());

            // Normaliza el rango a límites UTC por día (la columna es timestamptz).
            DateTime? desde = request.Desde.HasValue
                ? DateTime.SpecifyKind(request.Desde.Value.Date, DateTimeKind.Utc) : null;
            DateTime? hasta = request.Hasta.HasValue
                ? DateTime.SpecifyKind(request.Hasta.Value.Date.AddDays(1), DateTimeKind.Utc) : null;

            var rows = await _partidos.GetDbSet().AsNoTracking()
                .Where(p => p.Active && p.TorneoId == torneoId.Value)
                .Where(p => !desde.HasValue || p.FechaPartido >= desde.Value)
                .Where(p => !hasta.HasValue || p.FechaPartido < hasta.Value)
                .OrderBy(p => p.FechaPartido).ThenBy(p => p.Id)
                .Select(p => new Row
                {
                    Id = p.Id,
                    FechaPartido = p.FechaPartido,
                    Grupo = p.Grupo!.Nombre,
                    Estado = p.Estado,
                    LocalId = p.EquipoLocalId,
                    LocalNombre = p.EquipoLocal!.Nombre,
                    LocalBandera = p.EquipoLocal.UrlBandera,
                    VisitanteId = p.EquipoVisitanteId,
                    VisitanteNombre = p.EquipoVisitante!.Nombre,
                    VisitanteBandera = p.EquipoVisitante.UrlBandera,
                    ResultadoLocal = p.ResultadoLocalId,
                    ResultadoVisitante = p.ResultadoVisitanteId,
                    PtsExacto = p.TipoPartido!.PtsQuinelaResultadoExacto,
                    PtsAcertado = p.TipoPartido.PtsQuinelaResultadoAcertado
                }).ToListAsync(ct);

            // Predicciones activas del usuario autenticado en esta quiniela, indexadas por partido.
            var username = _currentUser.UserName;
            var preds = await _predicciones.GetDbSet().AsNoTracking()
                .Where(x => x.Active && x.Username == username && x.QuinielaId == request.QuinielaId)
                .ToListAsync(ct);
            var predByPartido = preds
                .GroupBy(x => x.PartidoId)
                .ToDictionary(g => g.Key, g => g.OrderByDescending(x => x.Id).First());

            var data = rows.Select(r =>
            {
                predByPartido.TryGetValue(r.Id, out var pred);

                int? puntos = null;
                string? categoria = null;
                // Solo se evalúan puntos si el partido tiene resultado y la predicción está completa.
                if (r.ResultadoLocal.HasValue && r.ResultadoVisitante.HasValue
                    && pred is not null && pred.Team1Resultado.HasValue && pred.Team2Resultado.HasValue)
                {
                    var exacto = pred.Team1Resultado.Value == r.ResultadoLocal.Value
                                 && pred.Team2Resultado.Value == r.ResultadoVisitante.Value;
                    var signoReal = Math.Sign(r.ResultadoLocal.Value - r.ResultadoVisitante.Value);
                    var signoPred = Math.Sign(pred.Team1Resultado.Value - pred.Team2Resultado.Value);

                    if (exacto) { categoria = "exacto"; puntos = r.PtsExacto; }
                    else if (signoReal == signoPred) { categoria = "acertado"; puntos = r.PtsAcertado; }
                    else { categoria = "ninguno"; puntos = 0; }
                }

                return new PartidoCalendarioDto
                {
                    Id = r.Id,
                    FechaPartido = r.FechaPartido,
                    Grupo = r.Grupo,
                    Estado = r.Estado,
                    Local = new EquipoCalendarioDto { Id = r.LocalId, Nombre = r.LocalNombre, UrlBandera = r.LocalBandera },
                    Visitante = new EquipoCalendarioDto { Id = r.VisitanteId, Nombre = r.VisitanteNombre, UrlBandera = r.VisitanteBandera },
                    ResultadoLocal = r.ResultadoLocal,
                    ResultadoVisitante = r.ResultadoVisitante,
                    Prediccion = pred is null ? null : new PrediccionResumenDto
                    {
                        Id = pred.Id,
                        Team1Resultado = pred.Team1Resultado,
                        Team2Resultado = pred.Team2Resultado
                    },
                    PuntosGanados = puntos,
                    CategoriaPuntos = categoria
                };
            }).ToList();

            return Result.Success(data);
        }

        // Proyección intermedia para calcular puntos en memoria.
        private sealed class Row
        {
            public int Id { get; set; }
            public DateTime FechaPartido { get; set; }
            public string Grupo { get; set; } = string.Empty;
            public char Estado { get; set; }
            public int LocalId { get; set; }
            public string LocalNombre { get; set; } = string.Empty;
            public string? LocalBandera { get; set; }
            public int VisitanteId { get; set; }
            public string VisitanteNombre { get; set; } = string.Empty;
            public string? VisitanteBandera { get; set; }
            public int? ResultadoLocal { get; set; }
            public int? ResultadoVisitante { get; set; }
            public int PtsExacto { get; set; }
            public int PtsAcertado { get; set; }
        }
    }
}

using MediatR;
using Microsoft.EntityFrameworkCore;
using Quinela.Application.Common.Abstractions;
using Quinela.Application.Common.Results;
using Quinela.Domain.Entities;

namespace Quinela.Application.Features.Predicciones
{
    public class EquipoPrediccionDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string? UrlBandera { get; set; }
    }

    /// <summary>Una predicción de un usuario en un partido TERMINADO, con sus puntos.</summary>
    public class PrediccionUsuarioDto
    {
        public int PartidoId { get; set; }
        public DateTime FechaPartido { get; set; }
        public string TipoPartido { get; set; } = string.Empty;
        public EquipoPrediccionDto Local { get; set; } = new();
        public EquipoPrediccionDto Visitante { get; set; } = new();

        // Predicción del usuario.
        public int? Team1Resultado { get; set; }
        public int? Team2Resultado { get; set; }

        // Resultado real del partido.
        public int? ResultadoLocal { get; set; }
        public int? ResultadoVisitante { get; set; }

        public int Puntos { get; set; }
        public string? Categoria { get; set; } // "exacto" | "acertado" | "ninguno"

        // Fecha/hora (UTC) en que se guardó la predicción (última actualización o creación).
        public DateTime GuardadaEn { get; set; }
    }

    // Predicciones de un usuario en una quiniela, SOLO de partidos terminados ('T'), con puntos.
    public sealed record GetPrediccionesUsuarioQuery(int QuinielaId, string Username)
        : IRequest<Result<List<PrediccionUsuarioDto>>>;

    internal sealed class GetPrediccionesUsuarioHandler
        : IRequestHandler<GetPrediccionesUsuarioQuery, Result<List<PrediccionUsuarioDto>>>
    {
        private readonly IRepository<Prediccion> _repo;
        public GetPrediccionesUsuarioHandler(IRepository<Prediccion> repo) => _repo = repo;

        public async Task<Result<List<PrediccionUsuarioDto>>> Handle(GetPrediccionesUsuarioQuery request, CancellationToken ct)
        {
            if (request.QuinielaId <= 0 || string.IsNullOrWhiteSpace(request.Username))
                return Result.Success(new List<PrediccionUsuarioDto>());

            var rows = await _repo.GetDbSet().AsNoTracking()
                .Where(x => x.Active && x.QuinielaId == request.QuinielaId && x.Username == request.Username
                    && x.Partido.Active && x.Partido.Estado == 'T'
                    && x.Partido.ResultadoLocalId != null && x.Partido.ResultadoVisitanteId != null)
                .OrderBy(x => x.Partido.FechaPartido).ThenBy(x => x.PartidoId)
                .Select(x => new Row
                {
                    PartidoId = x.PartidoId,
                    FechaPartido = x.Partido.FechaPartido,
                    Tipo = x.Partido.TipoPartido.Descripcion,
                    LocalNombre = x.Partido.EquipoLocal.Nombre,
                    LocalBandera = x.Partido.EquipoLocal.UrlBandera,
                    VisitanteNombre = x.Partido.EquipoVisitante.Nombre,
                    VisitanteBandera = x.Partido.EquipoVisitante.UrlBandera,
                    T1 = x.Team1Resultado,
                    T2 = x.Team2Resultado,
                    ResultadoLocal = x.Partido.ResultadoLocalId,
                    ResultadoVisitante = x.Partido.ResultadoVisitanteId,
                    PtsExacto = x.Partido.TipoPartido.PtsQuinelaResultadoExacto,
                    PtsAcertado = x.Partido.TipoPartido.PtsQuinelaResultadoAcertado,
                    GuardadaEn = x.UpdatedAt ?? x.CreatedAt
                })
                .ToListAsync(ct);

            var data = rows.Select(r =>
            {
                int puntos = 0;
                string? categoria = null;
                if (r.T1.HasValue && r.T2.HasValue && r.ResultadoLocal.HasValue && r.ResultadoVisitante.HasValue)
                {
                    var exacto = r.T1.Value == r.ResultadoLocal.Value && r.T2.Value == r.ResultadoVisitante.Value;
                    var signoReal = Math.Sign(r.ResultadoLocal.Value - r.ResultadoVisitante.Value);
                    var signoPred = Math.Sign(r.T1.Value - r.T2.Value);
                    if (exacto) { categoria = "exacto"; puntos = r.PtsExacto; }
                    else if (signoReal == signoPred) { categoria = "acertado"; puntos = r.PtsAcertado; }
                    else { categoria = "ninguno"; puntos = 0; }
                }

                return new PrediccionUsuarioDto
                {
                    PartidoId = r.PartidoId,
                    FechaPartido = r.FechaPartido,
                    TipoPartido = r.Tipo,
                    Local = new EquipoPrediccionDto { Nombre = r.LocalNombre, UrlBandera = r.LocalBandera },
                    Visitante = new EquipoPrediccionDto { Nombre = r.VisitanteNombre, UrlBandera = r.VisitanteBandera },
                    Team1Resultado = r.T1,
                    Team2Resultado = r.T2,
                    ResultadoLocal = r.ResultadoLocal,
                    ResultadoVisitante = r.ResultadoVisitante,
                    Puntos = puntos,
                    Categoria = categoria,
                    GuardadaEn = DateTime.SpecifyKind(r.GuardadaEn, DateTimeKind.Utc)
                };
            }).ToList();

            return Result.Success(data);
        }

        private sealed class Row
        {
            public int PartidoId { get; set; }
            public DateTime FechaPartido { get; set; }
            public string Tipo { get; set; } = string.Empty;
            public string LocalNombre { get; set; } = string.Empty;
            public string? LocalBandera { get; set; }
            public string VisitanteNombre { get; set; } = string.Empty;
            public string? VisitanteBandera { get; set; }
            public int? T1 { get; set; }
            public int? T2 { get; set; }
            public int? ResultadoLocal { get; set; }
            public int? ResultadoVisitante { get; set; }
            public int PtsExacto { get; set; }
            public int PtsAcertado { get; set; }
            public DateTime GuardadaEn { get; set; }
        }
    }
}

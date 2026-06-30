using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quinela.Application.Common.Abstractions;
using Quinela.Application.Features.Master.Partidos;
using Quinela.Domain.Entities;

namespace Quinela.Application.Features.AutomationMatch
{
    /// <summary>
    /// Paso 2: toma los partidos en curso (estado 'E') que tienen id del API y, para
    /// cada uno, consulta /get/game/{id}. Solo mira goles del local/visitante y el
    /// campo finished:
    ///  - Si NO terminó: actualiza los goles (solo si cambiaron) vía UpdatePartidoCommand,
    ///    que recalcula el ranking.
    ///  - Si terminó (finished = TRUE): pone el partido en 'T' con el marcador final,
    ///    también vía el CRUD para recalcular ranking + grupos. En 'T' deja de consultarse.
    /// El orquestador llama esto cada 30s para detectar goles nuevos.
    /// </summary>
    public sealed class MatchStatusVerificationService
    {
        private readonly IRepository<Partido> _partidos;
        private readonly IWorldCupApiClient _api;
        private readonly ISender _sender;
        private readonly ILogger<MatchStatusVerificationService> _logger;

        public MatchStatusVerificationService(IRepository<Partido> partidos, IWorldCupApiClient api,
            ISender sender, ILogger<MatchStatusVerificationService> logger)
        {
            _partidos = partidos;
            _api = api;
            _sender = sender;
            _logger = logger;
        }

        public async Task EjecutarAsync(CancellationToken ct)
        {
            // Partidos en curso con id del API (los 'T' ya no se consultan).
            var enCurso = await _partidos.GetDbSet().AsNoTracking()
                .Where(p => p.Active && p.Estado == 'E' && p.PartidoIdApi != null)
                .Select(p => new PartidoCmd(
                    p.Id, p.FechaPartido, p.TorneoId, p.GrupoId, p.FaseId, p.EquipoLocalId, p.EquipoVisitanteId,
                    p.TipoPartidoId, p.PartidoIdApi!, p.ResultadoLocalId, p.ResultadoVisitanteId, p.Active,
                    p.PartidoSeDefiniraEnPenales, p.PenalesAnotadosLocal, p.PenalesAnotadosVisitante,
                    p.EquipoGanadorId, p.PartidoGanadorLocalId, p.PartidoGanadorVisitanteId))
                .ToListAsync(ct);

            foreach (var p in enCurso)
            {
                GameApi? game;
                try
                {
                    game = await _api.GetGameAsync(p.PartidoIdApi, ct);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Falló la consulta del game {Api} (partido {Id}).", p.PartidoIdApi, p.Id);
                    continue;
                }
                if (game is null)
                {
                    _logger.LogWarning("Sin datos del game {Api} (partido {Id}).", p.PartidoIdApi, p.Id);
                    continue;
                }

                var nuevoEstado = game.Finished ? 'T' : 'E';
                var golesCambiaron = game.HomeScore != p.ResultadoLocal || game.AwayScore != p.ResultadoVisitante;

                // Si sigue en curso y no hubo gol nuevo, no hacemos nada (evita recálculos).
                if (!game.Finished && !golesCambiaron) continue;

                var res = await _sender.Send(new UpdatePartidoCommand(
                    p.Id, p.FechaPartido, p.TorneoId, p.GrupoId, p.FaseId, p.EquipoLocalId, p.EquipoVisitanteId,
                    p.TipoPartidoId, nuevoEstado, game.HomeScore, game.AwayScore, p.PartidoIdApi, p.Active,
                    p.PartidoSeDefiniraEnPenales, p.PenalesAnotadosLocal, p.PenalesAnotadosVisitante,
                    p.EquipoGanadorId, p.PartidoGanadorLocalId, p.PartidoGanadorVisitanteId, false, false), ct);

                if (res.IsFailure)
                    _logger.LogWarning("No se pudo actualizar el partido {Id}: {Error}", p.Id, res.Error.Message);
                else
                    _logger.LogInformation("Partido {Id} -> {Estado} {Home}-{Away}.", p.Id, nuevoEstado, game.HomeScore, game.AwayScore);
            }
        }

        private sealed record PartidoCmd(int Id, DateTime FechaPartido, int TorneoId, int? GrupoId, int FaseId,
            int? EquipoLocalId, int? EquipoVisitanteId, int TipoPartidoId, string? PartidoIdApi,
            int? ResultadoLocal, int? ResultadoVisitante, bool Active,
            bool? PartidoSeDefiniraEnPenales, int? PenalesAnotadosLocal, int? PenalesAnotadosVisitante,
            int? EquipoGanadorId, int? PartidoGanadorLocalId, int? PartidoGanadorVisitanteId);
    }
}

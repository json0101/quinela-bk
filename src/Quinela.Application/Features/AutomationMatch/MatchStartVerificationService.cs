using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quinela.Application.Common.Abstractions;
using Quinela.Application.Features.Master.Partidos;
using Quinela.Domain.Entities;

namespace Quinela.Application.Features.AutomationMatch
{
    /// <summary>
    /// Paso 1: detecta los partidos cuya hora de inicio ya pasó y los arranca.
    /// La fecha en la BD está en UTC; se compara contra DateTime.UtcNow (hora del
    /// servidor en UTC), así hace match con la fecha almacenada. Cuando el partido
    /// empezó, lo pone en estado 'E' (en curso) con marcador 0-0. Reusa
    /// UpdatePartidoCommand para que se recalculen grupos + ranking.
    /// </summary>
    public sealed class MatchStartVerificationService
    {
        private readonly IRepository<Partido> _partidos;
        private readonly ISender _sender;
        private readonly ILogger<MatchStartVerificationService> _logger;

        public MatchStartVerificationService(IRepository<Partido> partidos, ISender sender,
            ILogger<MatchStartVerificationService> logger)
        {
            _partidos = partidos;
            _sender = sender;
            _logger = logger;
        }

        public async Task EjecutarAsync(CancellationToken ct)
        {
            // Arranca 30s antes de la hora oficial: así el partido ya está en curso si hay
            // un gol temprano y se evita que los usuarios cambien su predicción por el delay.
            var ahoraUtc = DateTime.UtcNow.AddSeconds(30);

            // Partidos en 'Previa' activos cuya fecha/hora (UTC en BD) ya llegó (o llega en 30s).
            var porArrancar = await _partidos.GetDbSet().AsNoTracking()
                // Solo partidos de grupo: la eliminatoria se arma con el servicio de distribución.
                .Where(p => p.Active && p.Estado == 'P' && p.FechaPartido <= ahoraUtc)
                .Select(p => new PartidoCmd(
                    p.Id, p.FechaPartido, p.TorneoId, p.GrupoId!.Value, p.FaseId, p.EquipoLocalId!.Value, p.EquipoVisitanteId!.Value,
                    p.TipoPartidoId, p.PartidoIdApi, p.Active,
                    p.PartidoSeDefiniraEnPenales, p.PenalesAnotadosLocal, p.PenalesAnotadosVisitante,
                    p.EquipoGanadorId, p.PartidoGanadorLocalId, p.PartidoGanadorVisitanteId))
                .ToListAsync(ct);

            foreach (var p in porArrancar)
            {
                // Estado 'E' + marcador 0-0. Reusa el CRUD (dispara recálculo de ranking/grupos).
                var res = await _sender.Send(new UpdatePartidoCommand(
                    p.Id, p.FechaPartido, p.TorneoId, p.GrupoId, p.FaseId, p.EquipoLocalId, p.EquipoVisitanteId,
                    p.TipoPartidoId, 'E', 0, 0, p.PartidoIdApi, p.Active,
                    p.PartidoSeDefiniraEnPenales, p.PenalesAnotadosLocal, p.PenalesAnotadosVisitante,
                    p.EquipoGanadorId, p.PartidoGanadorLocalId, p.PartidoGanadorVisitanteId, false, false), ct);

                if (res.IsFailure)
                    _logger.LogWarning("No se pudo arrancar el partido {Id}: {Error}", p.Id, res.Error.Message);
                else
                    _logger.LogInformation("Partido {Id} arrancado (E, 0-0).", p.Id);
            }
        }

        private sealed record PartidoCmd(int Id, DateTime FechaPartido, int TorneoId, int GrupoId, int FaseId,
            int EquipoLocalId, int EquipoVisitanteId, int TipoPartidoId, string? PartidoIdApi, bool Active,
            bool? PartidoSeDefiniraEnPenales, int? PenalesAnotadosLocal, int? PenalesAnotadosVisitante,
            int? EquipoGanadorId, int? PartidoGanadorLocalId, int? PartidoGanadorVisitanteId);
    }
}

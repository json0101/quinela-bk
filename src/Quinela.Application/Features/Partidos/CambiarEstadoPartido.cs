using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quinela.Application.Common.Abstractions;
using Quinela.Application.Common.Results;
using Quinela.Domain.Entities;

namespace Quinela.Application.Features.Partidos
{
    public static class PartidoErrors
    {
        public static readonly Error NotFound = Error.NotFound("Partido.NotFound", "No se encontró el partido.");
        public static readonly Error TransicionInvalida = Error.Conflict(
            "Partido.TransicionInvalida",
            "Transición de estado no permitida. Solo se permite 'P' → 'E' → 'T'.");
    }

    // Cambia el estado del partido y dispara el recálculo de grupos y ranking.
    public sealed record CambiarEstadoPartidoCommand(int PartidoId, char NuevoEstado, int? ResultadoLocal, int? ResultadoVisitante)
        : IRequest<Result>;

    public sealed class CambiarEstadoPartidoValidator : AbstractValidator<CambiarEstadoPartidoCommand>
    {
        public CambiarEstadoPartidoValidator()
        {
            RuleFor(x => x.PartidoId).GreaterThan(0);
            RuleFor(x => x.NuevoEstado).Must(e => e == 'E' || e == 'T')
                .WithMessage("El nuevo estado debe ser 'E' (en curso) o 'T' (terminado).");
            RuleFor(x => x.ResultadoLocal).NotNull().GreaterThanOrEqualTo(0)
                .WithMessage("El resultado local es requerido y no puede ser negativo.");
            RuleFor(x => x.ResultadoVisitante).NotNull().GreaterThanOrEqualTo(0)
                .WithMessage("El resultado visitante es requerido y no puede ser negativo.");
        }
    }

    internal sealed class CambiarEstadoPartidoHandler : IRequestHandler<CambiarEstadoPartidoCommand, Result>
    {
        private readonly IRepository<Partido> _partidos;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUser _currentUser;
        private readonly IRankingService _ranking;

        public CambiarEstadoPartidoHandler(IRepository<Partido> partidos, IUnitOfWork uow,
            ICurrentUser currentUser, IRankingService ranking)
        { _partidos = partidos; _uow = uow; _currentUser = currentUser; _ranking = ranking; }

        public async Task<Result> Handle(CambiarEstadoPartidoCommand cmd, CancellationToken ct)
        {
            var partido = await _partidos.GetDbSet()
                .Include(p => p.TipoPartido)
                .FirstOrDefaultAsync(x => x.Id == cmd.PartidoId, ct);
            if (partido is null) return Result.Failure(PartidoErrors.NotFound);

            // Restricción: solo P->E y E->T. Desde 'T' ya no se actualiza nada.
            var transicionValida =
                (partido.Estado == 'P' && cmd.NuevoEstado == 'E') ||
                (partido.Estado == 'E' && cmd.NuevoEstado == 'T');
            if (!transicionValida) return Result.Failure(PartidoErrors.TransicionInvalida);

            var rl = cmd.ResultadoLocal!.Value;
            var rv = cmd.ResultadoVisitante!.Value;
            var signo = Math.Sign(rl - rv);

            partido.ResultadoLocalId = rl;
            partido.ResultadoVisitanteId = rv;
            partido.PtsLocal = signo > 0 ? partido.TipoPartido!.PtsPartidoVictoria : signo == 0 ? partido.TipoPartido!.PtsPartidoEmpate : 0;
            partido.PtsVisitante = signo < 0 ? partido.TipoPartido!.PtsPartidoVictoria : signo == 0 ? partido.TipoPartido!.PtsPartidoEmpate : 0;
            partido.Estado = cmd.NuevoEstado;
            partido.UpdatedAt = DateTime.UtcNow;
            partido.UpdatedBy = _currentUser.UserName;

            await _uow.SaveChangesAsync(ct);

            // Recálculo separado (posiciones del torneo + ranking de sus quinielas).
            await _ranking.RecalcularAsync(partido.TorneoId, ct);

            return Result.Success();
        }
    }
}

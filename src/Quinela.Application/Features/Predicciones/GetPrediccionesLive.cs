using MediatR;
using Microsoft.EntityFrameworkCore;
using Quinela.Application.Common.Abstractions;
using Quinela.Application.Common.Results;
using Quinela.Domain.Entities;

namespace Quinela.Application.Features.Predicciones
{
    /// <summary>Predicción de un usuario para mostrar en vivo (todas las de un partido/quiniela).</summary>
    public class PrediccionLiveDto
    {
        public string Username { get; set; } = string.Empty;
        public int? Team1Resultado { get; set; }
        public int? Team2Resultado { get; set; }
    }

    // Todas las predicciones activas de TODOS los usuarios para un partido en una quiniela.
    public sealed record GetPrediccionesLiveQuery(int QuinielaId, int PartidoId)
        : IRequest<Result<List<PrediccionLiveDto>>>;

    internal sealed class GetPrediccionesLiveHandler
        : IRequestHandler<GetPrediccionesLiveQuery, Result<List<PrediccionLiveDto>>>
    {
        private readonly IRepository<Prediccion> _repo;
        public GetPrediccionesLiveHandler(IRepository<Prediccion> repo) => _repo = repo;

        public async Task<Result<List<PrediccionLiveDto>>> Handle(GetPrediccionesLiveQuery request, CancellationToken ct)
        {
            if (request.QuinielaId <= 0 || request.PartidoId <= 0)
                return Result.Success(new List<PrediccionLiveDto>());

            var rows = await _repo.GetDbSet().AsNoTracking()
                .Where(x => x.Active && x.QuinielaId == request.QuinielaId && x.PartidoId == request.PartidoId)
                .OrderBy(x => x.Username)
                .Select(x => new PrediccionLiveDto
                {
                    Username = x.Username,
                    Team1Resultado = x.Team1Resultado,
                    Team2Resultado = x.Team2Resultado
                })
                .ToListAsync(ct);

            return Result.Success(rows);
        }
    }
}

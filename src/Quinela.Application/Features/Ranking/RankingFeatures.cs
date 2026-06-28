using MediatR;
using Microsoft.EntityFrameworkCore;
using Quinela.Application.Common.Abstractions;
using Quinela.Application.Common.Results;
using RankingEntity = Quinela.Domain.Entities.Ranking;

namespace Quinela.Application.Features.Ranking
{
    // El ranking se calcula por lógica; aquí solo se consulta (no se edita por endpoint).

    // ----- GetAll por quiniela -----
    public sealed record GetAllRankingQuery(int QuinielaId) : IRequest<Result<List<RankingDto>>>;

    internal sealed class GetAllRankingHandler : IRequestHandler<GetAllRankingQuery, Result<List<RankingDto>>>
    {
        private readonly IRepository<RankingEntity> _repo;
        public GetAllRankingHandler(IRepository<RankingEntity> repo) => _repo = repo;

        public async Task<Result<List<RankingDto>>> Handle(GetAllRankingQuery request, CancellationToken ct)
        {
            var rows = await _repo.GetDbSet().AsNoTracking()
                .Where(x => x.Active && x.QuinielaId == request.QuinielaId)
                .OrderByDescending(x => x.Pts)
                .ThenByDescending(x => x.ResultadoExacto)
                .Select(x => new RankingDto
                {
                    Id = x.Id,
                    Usuario = x.Usuario,
                    Pts = x.Pts,
                    ResultadoAtinado = x.ResultadoAtinado,
                    ResultadoExacto = x.ResultadoExacto,
                    Active = x.Active,
                    CreatedAt = x.CreatedAt,
                    CreatedBy = x.CreatedBy,
                    UpdatedAt = x.UpdatedAt,
                    UpdatedBy = x.UpdatedBy
                })
                .ToListAsync(ct);

            AsignarPosiciones(rows);
            return Result.Success(rows);
        }

        // Asigna la posición usando ranking de competición estándar (1,1,3...):
        // dos filas empatan (misma posición) si coinciden en Pts y exactos (los atinados
        // NO desempatan); cuando hay empate, la siguiente posición se salta.
        // La lista debe venir ya ordenada por el criterio de desempate.
        internal static void AsignarPosiciones(List<RankingDto> rows)
        {
            for (int i = 0; i < rows.Count; i++)
            {
                var actual = rows[i];
                var anterior = i > 0 ? rows[i - 1] : null;

                bool empata = anterior != null
                    && anterior.Pts == actual.Pts
                    && anterior.ResultadoExacto == actual.ResultadoExacto;

                // Si empata con el anterior, comparte su posición; si no, ocupa su
                // índice (1-based), lo que "salta" las posiciones consumidas por empates.
                actual.Posicion = empata ? anterior!.Posicion : i + 1;
            }
        }
    }
}

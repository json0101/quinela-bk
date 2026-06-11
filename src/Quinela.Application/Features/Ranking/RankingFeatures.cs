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
                .ThenByDescending(x => x.ResultadoAtinado)
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
            return Result.Success(rows);
        }
    }
}

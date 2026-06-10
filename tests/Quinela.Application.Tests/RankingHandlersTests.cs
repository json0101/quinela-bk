using Quinela.Application.Features.Ranking;
using Quinela.Infrastructure.Repositories;
using RankingEntity = Quinela.Domain.Entities.Ranking;

namespace Quinela.Application.Tests;

public class RankingHandlersTests
{
    private static RankingEntity Seed(string usuario, int pts, int exacto, int atinado, bool active) => new()
    {
        Usuario = usuario,
        Pts = pts,
        ResultadoExacto = exacto,
        ResultadoAtinado = atinado,
        Active = active,
        CreatedAt = DateTime.UtcNow,
        CreatedBy = "seed"
    };

    [Fact]
    public async Task GetAll_OrdersByPtsThenExactoThenAtinado_AndSkipsInactive()
    {
        using var ctx = TestHelpers.NewContext(nameof(GetAll_OrdersByPtsThenExactoThenAtinado_AndSkipsInactive));
        ctx.Rankings.AddRange(
            Seed("ana", pts: 10, exacto: 2, atinado: 4, active: true),
            Seed("beto", pts: 20, exacto: 1, atinado: 3, active: true),
            Seed("cita", pts: 20, exacto: 5, atinado: 1, active: true),
            Seed("dario", pts: 99, exacto: 9, atinado: 9, active: false));
        ctx.SaveChanges();

        var handler = new GetAllRankingHandler(new Repository<RankingEntity>(ctx));
        var result = await handler.Handle(new GetAllRankingQuery(), CancellationToken.None);

        Assert.True(result.IsSuccess);
        // El inactivo (dario) se excluye; orden: mayor pts, desempate por exacto.
        Assert.Equal(new[] { "cita", "beto", "ana" }, result.Value.Select(x => x.Usuario).ToArray());
    }
}

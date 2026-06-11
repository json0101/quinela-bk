using Microsoft.EntityFrameworkCore;
using Quinela.Application.Common.Abstractions;
using Quinela.Application.Features.Partidos;
using Quinela.Domain.Entities;
using Quinela.Infrastructure.Persistence;
using Quinela.Infrastructure.Repositories;

namespace Quinela.Application.Tests;

public class CambiarEstadoPartidoTests
{
    // Doble de prueba: solo registra si se disparó el recálculo.
    private sealed class FakeRankingService : IRankingService
    {
        public int Veces { get; private set; }
        public int UltimoTorneoId { get; private set; }
        public Task RecalcularAsync(int torneoId, CancellationToken ct = default)
        {
            Veces++;
            UltimoTorneoId = torneoId;
            return Task.CompletedTask;
        }
    }

    private static (CambiarEstadoPartidoHandler handler, FakeRankingService ranking) NewHandler(QuinelaContext ctx)
    {
        var ranking = new FakeRankingService();
        var handler = new CambiarEstadoPartidoHandler(
            new Repository<Partido>(ctx), new UnitOfWork(ctx), TestHelpers.CurrentUser("admin"), ranking);
        return (handler, ranking);
    }

    private static async Task SetEstado(QuinelaContext ctx, int id, char estado)
    {
        var p = await ctx.Partidos.FirstAsync(x => x.Id == id);
        p.Estado = estado;
        await ctx.SaveChangesAsync();
    }

    [Fact]
    public async Task PaE_Actualiza_Y_DisparaRecalculo()
    {
        using var ctx = TestHelpers.NewContext(nameof(PaE_Actualiza_Y_DisparaRecalculo));
        var (handler, ranking) = NewHandler(ctx);

        var result = await handler.Handle(new CambiarEstadoPartidoCommand(1, 'E', 0, 0), CancellationToken.None);

        Assert.True(result.IsSuccess);
        var p = await ctx.Partidos.AsNoTracking().FirstAsync(x => x.Id == 1);
        Assert.Equal('E', p.Estado);
        Assert.Equal(1, ranking.Veces);
    }

    [Fact]
    public async Task EaT_Actualiza_Y_DisparaRecalculo()
    {
        using var ctx = TestHelpers.NewContext(nameof(EaT_Actualiza_Y_DisparaRecalculo));
        await SetEstado(ctx, 1, 'E');
        var (handler, ranking) = NewHandler(ctx);

        var result = await handler.Handle(new CambiarEstadoPartidoCommand(1, 'T', 2, 1), CancellationToken.None);

        Assert.True(result.IsSuccess);
        var p = await ctx.Partidos.AsNoTracking().FirstAsync(x => x.Id == 1);
        Assert.Equal('T', p.Estado);
        Assert.Equal(2, p.ResultadoLocalId);
        Assert.Equal(1, p.ResultadoVisitanteId);
        Assert.Equal(1, ranking.Veces);
    }

    [Fact]
    public async Task PaT_Directo_NoPermitido()
    {
        using var ctx = TestHelpers.NewContext(nameof(PaT_Directo_NoPermitido));
        var (handler, ranking) = NewHandler(ctx);

        var result = await handler.Handle(new CambiarEstadoPartidoCommand(1, 'T', 2, 1), CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(PartidoErrors.TransicionInvalida, result.Error);
        Assert.Equal(0, ranking.Veces); // no recalcula
        var p = await ctx.Partidos.AsNoTracking().FirstAsync(x => x.Id == 1);
        Assert.Equal('P', p.Estado); // sin cambios
    }

    [Fact]
    public async Task DesdeT_YaNoActualiza()
    {
        using var ctx = TestHelpers.NewContext(nameof(DesdeT_YaNoActualiza));
        await SetEstado(ctx, 1, 'T');
        var (handler, ranking) = NewHandler(ctx);

        var result = await handler.Handle(new CambiarEstadoPartidoCommand(1, 'E', 0, 0), CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(PartidoErrors.TransicionInvalida, result.Error);
        Assert.Equal(0, ranking.Veces);
    }
}

using Microsoft.EntityFrameworkCore;
using Quinela.Application.Features.Predicciones;
using Quinela.Domain.Entities;
using Quinela.Infrastructure.Repositories;

namespace Quinela.Application.Tests;

public class UpsertPrediccionTests
{
    private const string User = "tester";

    private static UpsertPrediccionHandler NewHandler(Quinela.Infrastructure.Persistence.QuinelaContext ctx) =>
        new(new Repository<Prediccion>(ctx), new Repository<Partido>(ctx),
            new UnitOfWork(ctx), TestHelpers.CurrentUser(User), TestHelpers.UserService(User));

    [Fact]
    public async Task Upsert_WhenNoPrediccion_Creates()
    {
        using var ctx = TestHelpers.NewContext(nameof(Upsert_WhenNoPrediccion_Creates));
        var result = await NewHandler(ctx).Handle(new UpsertPrediccionCommand(1, 2, 1), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Single(ctx.Predicciones);
        Assert.Equal(2, result.Value.Team1Resultado);
        Assert.Equal(1, result.Value.Team2Resultado);
    }

    [Fact]
    public async Task Upsert_WithOnlyLocal_SavesVisitanteAsNull_ThenUpdates()
    {
        using var ctx = TestHelpers.NewContext(nameof(Upsert_WithOnlyLocal_SavesVisitanteAsNull_ThenUpdates));

        // Primero solo el local; el visitante queda en null.
        var first = await NewHandler(ctx).Handle(new UpsertPrediccionCommand(1, 2, null), CancellationToken.None);
        Assert.True(first.IsSuccess);
        Assert.Equal(2, first.Value.Team1Resultado);
        Assert.Null(first.Value.Team2Resultado);

        // Luego se ingresa el visitante: misma fila, ahora completa.
        var second = await NewHandler(ctx).Handle(new UpsertPrediccionCommand(1, 2, 1), CancellationToken.None);
        Assert.True(second.IsSuccess);
        Assert.Equal(first.Value.Id, second.Value.Id);
        Assert.Equal(1, second.Value.Team2Resultado);
        Assert.Single(ctx.Predicciones);
    }

    [Fact]
    public async Task Upsert_WhenExisting_UpdatesSameRow()
    {
        using var ctx = TestHelpers.NewContext(nameof(Upsert_WhenExisting_UpdatesSameRow));
        var first = await NewHandler(ctx).Handle(new UpsertPrediccionCommand(1, 0, 0), CancellationToken.None);
        var second = await NewHandler(ctx).Handle(new UpsertPrediccionCommand(1, 3, 2), CancellationToken.None);

        Assert.True(second.IsSuccess);
        Assert.Equal(first.Value.Id, second.Value.Id); // misma fila (no duplica)
        Assert.Single(ctx.Predicciones);
        Assert.Equal(3, second.Value.Team1Resultado);
        Assert.Equal(2, second.Value.Team2Resultado);
    }

    [Fact]
    public async Task Upsert_WhenPartidoNotInPrevia_ReturnsPartidoCerrado()
    {
        using var ctx = TestHelpers.NewContext(nameof(Upsert_WhenPartidoNotInPrevia_ReturnsPartidoCerrado));
        // El partido 1 deja de estar en previa ('T' = terminado).
        var partido = await ctx.Partidos.FirstAsync(x => x.Id == 1);
        partido.Estado = 'T';
        await ctx.SaveChangesAsync();

        var result = await NewHandler(ctx).Handle(new UpsertPrediccionCommand(1, 1, 1), CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(PrediccionErrors.PartidoCerrado, result.Error);
        Assert.Empty(ctx.Predicciones);
    }
}

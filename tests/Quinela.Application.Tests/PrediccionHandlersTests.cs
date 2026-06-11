using Microsoft.EntityFrameworkCore;
using Quinela.Application.Features.Predicciones;
using Quinela.Domain.Entities;
using Quinela.Infrastructure.Persistence;
using Quinela.Infrastructure.Repositories;

namespace Quinela.Application.Tests;

public class PrediccionHandlersTests
{
    private const string User = "tester";
    private const int Qid = 1; // quiniela sembrada

    private static Prediccion Seed(QuinelaContext ctx, int partidoId, string username, bool active)
    {
        var p = new Prediccion
        {
            QuinielaId = Qid,
            PartidoId = partidoId,
            Team1Resultado = 1,
            Team2Resultado = 0,
            Username = username,
            Active = active,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = username
        };
        ctx.Predicciones.Add(p);
        ctx.SaveChanges();
        return p;
    }

    private static CreatePrediccionHandler NewCreate(QuinelaContext ctx, string user, string existingUser) =>
        new(new Repository<Prediccion>(ctx), new Repository<Partido>(ctx), new Repository<Quiniela>(ctx),
            new UnitOfWork(ctx), TestHelpers.CurrentUser(user), TestHelpers.UserService(existingUser));

    [Fact]
    public async Task Create_WithValidPartidoAndUser_PersistsPrediccion()
    {
        using var ctx = TestHelpers.NewContext(nameof(Create_WithValidPartidoAndUser_PersistsPrediccion));
        var handler = NewCreate(ctx, User, User);

        var result = await handler.Handle(new CreatePrediccionCommand(Qid, 1, 2, 1, true), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(User, result.Value.Username);
        Assert.Equal(2, result.Value.Team1Resultado);
        Assert.Single(ctx.Predicciones);
    }

    [Fact]
    public async Task Create_WhenPartidoMissing_ReturnsPartidoNotFound()
    {
        using var ctx = TestHelpers.NewContext(nameof(Create_WhenPartidoMissing_ReturnsPartidoNotFound));
        var handler = NewCreate(ctx, User, User);

        var result = await handler.Handle(new CreatePrediccionCommand(Qid, 99999, 1, 1, true), CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(PrediccionErrors.PartidoNotFound, result.Error);
        Assert.Empty(ctx.Predicciones);
    }

    [Fact]
    public async Task Create_WhenQuinielaMissing_ReturnsQuinielaNotFound()
    {
        using var ctx = TestHelpers.NewContext(nameof(Create_WhenQuinielaMissing_ReturnsQuinielaNotFound));
        var handler = NewCreate(ctx, User, User);

        var result = await handler.Handle(new CreatePrediccionCommand(9999, 1, 1, 1, true), CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(PrediccionErrors.QuinielaNotFound, result.Error);
        Assert.Empty(ctx.Predicciones);
    }

    [Fact]
    public async Task Create_WhenUserNotInUserApp_ReturnsUserNotFound()
    {
        using var ctx = TestHelpers.NewContext(nameof(Create_WhenUserNotInUserApp_ReturnsUserNotFound));
        var handler = NewCreate(ctx, User, "otro");

        var result = await handler.Handle(new CreatePrediccionCommand(Qid, 1, 1, 1, true), CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(PrediccionErrors.UserNotFound, result.Error);
        Assert.Empty(ctx.Predicciones);
    }

    [Fact]
    public async Task GetAll_ReturnsOnlyActive()
    {
        using var ctx = TestHelpers.NewContext(nameof(GetAll_ReturnsOnlyActive));
        Seed(ctx, 1, User, active: true);
        Seed(ctx, 2, User, active: false);

        var handler = new GetAllPrediccionesHandler(new Repository<Prediccion>(ctx));
        var result = await handler.Handle(new GetAllPrediccionesQuery(Qid), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Single(result.Value);
        Assert.All(result.Value, x => Assert.True(x.Active));
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        using var ctx = TestHelpers.NewContext(nameof(GetById_WhenMissing_ReturnsNotFound));
        var handler = new GetPrediccionByIdHandler(new Repository<Prediccion>(ctx));

        var result = await handler.Handle(new GetPrediccionByIdQuery(123), CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(PrediccionErrors.NotFound, result.Error);
    }

    [Fact]
    public async Task Update_ChangesValues()
    {
        using var ctx = TestHelpers.NewContext(nameof(Update_ChangesValues));
        var entity = Seed(ctx, 1, User, active: true);

        var handler = new UpdatePrediccionHandler(
            new Repository<Prediccion>(ctx), new Repository<Partido>(ctx),
            new UnitOfWork(ctx), TestHelpers.CurrentUser(User));

        var result = await handler.Handle(new UpdatePrediccionCommand(entity.Id, Qid, 2, 3, 4, true), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.PartidoId);
        Assert.Equal(3, result.Value.Team1Resultado);
        Assert.Equal(4, result.Value.Team2Resultado);
        Assert.NotNull(result.Value.UpdatedAt);
    }

    [Fact]
    public async Task Delete_SoftDeletes_RowRemainsButInactive()
    {
        using var ctx = TestHelpers.NewContext(nameof(Delete_SoftDeletes_RowRemainsButInactive));
        var entity = Seed(ctx, 1, User, active: true);

        var handler = new DeletePrediccionHandler(
            new Repository<Prediccion>(ctx), new Repository<Partido>(ctx), new UnitOfWork(ctx), TestHelpers.CurrentUser(User));

        var result = await handler.Handle(new DeletePrediccionCommand(entity.Id), CancellationToken.None);

        Assert.True(result.IsSuccess);
        var stored = await ctx.Predicciones.AsNoTracking().SingleAsync(x => x.Id == entity.Id);
        Assert.False(stored.Active);
        Assert.Single(ctx.Predicciones);
    }

    private static async Task CerrarPartido(QuinelaContext ctx, int partidoId)
    {
        var partido = await ctx.Partidos.FirstAsync(x => x.Id == partidoId);
        partido.Estado = 'T';
        await ctx.SaveChangesAsync();
    }

    [Fact]
    public async Task Create_WhenPartidoNotInPrevia_ReturnsPartidoCerrado()
    {
        using var ctx = TestHelpers.NewContext(nameof(Create_WhenPartidoNotInPrevia_ReturnsPartidoCerrado));
        await CerrarPartido(ctx, 1);
        var handler = NewCreate(ctx, User, User);

        var result = await handler.Handle(new CreatePrediccionCommand(Qid, 1, 1, 0, true), CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(PrediccionErrors.PartidoCerrado, result.Error);
        Assert.Empty(ctx.Predicciones);
    }

    [Fact]
    public async Task Update_WhenPartidoNotInPrevia_ReturnsPartidoCerrado()
    {
        using var ctx = TestHelpers.NewContext(nameof(Update_WhenPartidoNotInPrevia_ReturnsPartidoCerrado));
        var entity = Seed(ctx, 1, User, active: true);
        await CerrarPartido(ctx, 1);

        var handler = new UpdatePrediccionHandler(
            new Repository<Prediccion>(ctx), new Repository<Partido>(ctx),
            new UnitOfWork(ctx), TestHelpers.CurrentUser(User));

        var result = await handler.Handle(new UpdatePrediccionCommand(entity.Id, Qid, 1, 5, 5, true), CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(PrediccionErrors.PartidoCerrado, result.Error);
    }

    [Fact]
    public async Task Delete_WhenPartidoNotInPrevia_ReturnsPartidoCerrado()
    {
        using var ctx = TestHelpers.NewContext(nameof(Delete_WhenPartidoNotInPrevia_ReturnsPartidoCerrado));
        var entity = Seed(ctx, 1, User, active: true);
        await CerrarPartido(ctx, 1);

        var handler = new DeletePrediccionHandler(
            new Repository<Prediccion>(ctx), new Repository<Partido>(ctx), new UnitOfWork(ctx), TestHelpers.CurrentUser(User));

        var result = await handler.Handle(new DeletePrediccionCommand(entity.Id), CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(PrediccionErrors.PartidoCerrado, result.Error);
        var stored = await ctx.Predicciones.AsNoTracking().SingleAsync(x => x.Id == entity.Id);
        Assert.True(stored.Active);
    }
}

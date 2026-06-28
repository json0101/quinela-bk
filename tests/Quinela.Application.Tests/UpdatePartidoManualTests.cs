using Microsoft.EntityFrameworkCore;
using Quinela.Application.Common.Abstractions;
using Quinela.Application.Features.Master.Partidos;
using Quinela.Domain.Entities;
using Quinela.Infrastructure.Persistence;
using Quinela.Infrastructure.Repositories;

namespace Quinela.Application.Tests;

/// <summary>
/// Edición manual del partido (estado + goles): fija el marcador y los puntos según el estado
/// y dispara el recálculo del ranking. El recálculo en sí se prueba aparte (RankingServiceTests).
/// </summary>
public class UpdatePartidoManualTests
{
    private sealed class FakeRankingService : IRankingService
    {
        public int Veces { get; private set; }
        public Task RecalcularAsync(int torneoId, CancellationToken ct = default) { Veces++; return Task.CompletedTask; }
    }

    private sealed class FakeEliminatoria : Quinela.Application.Features.Eliminatoria.IDistributionEliminatoryWorldCup2026
    {
        private static readonly Quinela.Application.Features.Eliminatoria.BracketPreviewDto Vacio = new(0, new());
        public Task<Quinela.Application.Features.Eliminatoria.BracketPreviewDto> PreviewAsync(int torneoId, CancellationToken ct = default) => Task.FromResult(Vacio);
        public Task<Quinela.Application.Features.Eliminatoria.BracketPreviewDto> RecalcularAsync(int torneoId, CancellationToken ct = default) => Task.FromResult(Vacio);
    }

    private static (UpdatePartidoHandler handler, FakeRankingService ranking) NewHandler(QuinelaContext ctx)
    {
        var ranking = new FakeRankingService();
        var rel = new PartidoRelacionesValidator(
            new Repository<Torneo>(ctx), new Repository<Grupo>(ctx), new Repository<Equipo>(ctx), new Repository<TipoPartido>(ctx),
            new Repository<Fase>(ctx), new Repository<Partido>(ctx));
        var handler = new UpdatePartidoHandler(
            new Repository<Partido>(ctx), new Repository<TipoPartido>(ctx), rel, ranking, new FakeEliminatoria(),
            new UnitOfWork(ctx), TestHelpers.CurrentUser("admin"));
        return (handler, ranking);
    }

    private static UpdatePartidoCommand Cmd(Partido p, char estado, int? rl, int? rv) =>
        new(p.Id, p.FechaPartido, p.TorneoId, p.GrupoId, p.FaseId, p.EquipoLocalId, p.EquipoVisitanteId, p.TipoPartidoId, estado, rl, rv, p.PartidoIdApi, p.Active,
            null, null, null, null, null, null);

    [Fact]
    public async Task Terminado_FijaGolesYPuntosDeVictoria_YRecalcula()
    {
        using var ctx = TestHelpers.NewContext(nameof(Terminado_FijaGolesYPuntosDeVictoria_YRecalcula));
        var partido = await ctx.Partidos.AsNoTracking().FirstAsync(x => x.Id == 1);
        var tipo = await ctx.TiposPartido.AsNoTracking().FirstAsync(t => t.Id == partido.TipoPartidoId);
        var (handler, ranking) = NewHandler(ctx);

        var result = await handler.Handle(Cmd(partido, 'T', 2, 1), CancellationToken.None);

        Assert.True(result.IsSuccess);
        var p = await ctx.Partidos.AsNoTracking().FirstAsync(x => x.Id == 1);
        Assert.Equal('T', p.Estado);
        Assert.Equal(2, p.ResultadoLocalId);
        Assert.Equal(1, p.ResultadoVisitanteId);
        Assert.Equal(tipo.PtsPartidoVictoria, p.PtsLocal); // gana el local
        Assert.Equal(0, p.PtsVisitante);
        Assert.Equal(1, ranking.Veces);
    }

    [Fact]
    public async Task Empate_AsignaPuntosDeEmpateAAmbos()
    {
        using var ctx = TestHelpers.NewContext(nameof(Empate_AsignaPuntosDeEmpateAAmbos));
        var partido = await ctx.Partidos.AsNoTracking().FirstAsync(x => x.Id == 1);
        var tipo = await ctx.TiposPartido.AsNoTracking().FirstAsync(t => t.Id == partido.TipoPartidoId);
        var (handler, _) = NewHandler(ctx);

        var result = await handler.Handle(Cmd(partido, 'T', 1, 1), CancellationToken.None);

        Assert.True(result.IsSuccess);
        var p = await ctx.Partidos.AsNoTracking().FirstAsync(x => x.Id == 1);
        Assert.Equal(tipo.PtsPartidoEmpate, p.PtsLocal);
        Assert.Equal(tipo.PtsPartidoEmpate, p.PtsVisitante);
    }

    [Fact]
    public async Task VolverAPrevia_LimpiaMarcadorYPuntos_YRecalcula()
    {
        using var ctx = TestHelpers.NewContext(nameof(VolverAPrevia_LimpiaMarcadorYPuntos_YRecalcula));
        var partido = await ctx.Partidos.AsNoTracking().FirstAsync(x => x.Id == 1);
        var (handler, ranking) = NewHandler(ctx);

        // 1) Manual a Terminado 3-0.
        await handler.Handle(Cmd(partido, 'T', 3, 0), CancellationToken.None);
        // 2) Corrección: regresar a Previa.
        var result = await handler.Handle(Cmd(partido, 'P', null, null), CancellationToken.None);

        Assert.True(result.IsSuccess);
        var p = await ctx.Partidos.AsNoTracking().FirstAsync(x => x.Id == 1);
        Assert.Equal('P', p.Estado);
        Assert.Null(p.ResultadoLocalId);
        Assert.Null(p.ResultadoVisitanteId);
        Assert.Null(p.PtsLocal);
        Assert.Null(p.PtsVisitante);
        Assert.Equal(2, ranking.Veces); // recalcula en cada edición manual
    }

    [Fact]
    public async Task Terminado_SinGoles_FallaValidacion()
    {
        using var ctx = TestHelpers.NewContext(nameof(Terminado_SinGoles_FallaValidacion));
        var partido = await ctx.Partidos.AsNoTracking().FirstAsync(x => x.Id == 1);
        var validator = new UpdatePartidoValidator();

        var resultado = validator.Validate(Cmd(partido, 'T', null, null));

        Assert.False(resultado.IsValid);
    }
}

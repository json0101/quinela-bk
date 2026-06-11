using Microsoft.EntityFrameworkCore;
using Quinela.Domain.Entities;
using Quinela.Infrastructure.Persistence;
using Quinela.Infrastructure.Repositories;

namespace Quinela.Application.Tests;

public class RankingServiceTests
{
    private const int Qid = 1;   // quiniela sembrada del torneo 1
    private const int TorneoId = 1;

    private static Quinela.Application.Features.Ranking.RankingService NewService(QuinelaContext ctx) =>
        new(new Repository<Partido>(ctx), new Repository<Prediccion>(ctx),
            new Repository<GrupoEquipo>(ctx), new Repository<Ranking>(ctx),
            new Repository<Quiniela>(ctx), new UnitOfWork(ctx));

    // Deja el tipo de partido con puntajes reales y el partido 1 terminado 2-1.
    private static async Task PrepararPartido1Terminado(QuinelaContext ctx)
    {
        var tipo = await ctx.TiposPartido.FirstAsync();
        tipo.PtsPartidoVictoria = 3;
        tipo.PtsPartidoEmpate = 1;
        tipo.PtsQuinelaResultadoExacto = 5;
        tipo.PtsQuinelaResultadoAcertado = 2;

        var p1 = await ctx.Partidos.FirstAsync(x => x.Id == 1); // grupo A: local equipo 1 vs visitante equipo 2
        p1.Estado = 'T';
        p1.ResultadoLocalId = 2;
        p1.ResultadoVisitanteId = 1;
        await ctx.SaveChangesAsync();
    }

    private static Prediccion Pred(string user, int t1, int t2) => new()
    {
        QuinielaId = Qid,
        PartidoId = 1,
        Team1Resultado = t1,
        Team2Resultado = t2,
        Username = user,
        Active = true,
        CreatedAt = DateTime.UtcNow,
        CreatedBy = "test",
    };

    [Fact]
    public async Task Recalcular_SumaPuntosDeQuinielaSegunTipo()
    {
        using var ctx = TestHelpers.NewContext(nameof(Recalcular_SumaPuntosDeQuinielaSegunTipo));
        await PrepararPartido1Terminado(ctx);
        ctx.Predicciones.AddRange(
            Pred("ana", 2, 1),   // exacto -> 5 pts
            Pred("beto", 3, 0),  // acertó local gana (no exacto) -> 2 pts
            Pred("caro", 0, 1)); // falló (visitante) -> 0 pts
        await ctx.SaveChangesAsync();

        await NewService(ctx).RecalcularAsync(TorneoId);

        var ana = await ctx.Rankings.AsNoTracking().FirstAsync(r => r.Usuario == "ana");
        Assert.Equal(5, ana.Pts);
        Assert.Equal(1, ana.ResultadoExacto);
        Assert.Equal(0, ana.ResultadoAtinado);

        var beto = await ctx.Rankings.AsNoTracking().FirstAsync(r => r.Usuario == "beto");
        Assert.Equal(2, beto.Pts);
        Assert.Equal(0, beto.ResultadoExacto);
        Assert.Equal(1, beto.ResultadoAtinado);

        var caro = await ctx.Rankings.AsNoTracking().FirstAsync(r => r.Usuario == "caro");
        Assert.Equal(0, caro.Pts);
        Assert.Equal(0, caro.ResultadoExacto);
        Assert.Equal(0, caro.ResultadoAtinado);
    }

    [Fact]
    public async Task Recalcular_ActualizaTablaDeGrupos()
    {
        using var ctx = TestHelpers.NewContext(nameof(Recalcular_ActualizaTablaDeGrupos));
        await PrepararPartido1Terminado(ctx);

        await NewService(ctx).RecalcularAsync(TorneoId);

        var ganador = await ctx.GruposEquipos.AsNoTracking().FirstAsync(x => x.GrupoId == 1 && x.EquipoId == 1);
        Assert.Equal(3, ganador.Pts);
        Assert.Equal(2, ganador.GF);
        Assert.Equal(1, ganador.GC);
        Assert.Equal(1, ganador.Diff);
        Assert.Equal(1, ganador.Posicion);

        var perdedor = await ctx.GruposEquipos.AsNoTracking().FirstAsync(x => x.GrupoId == 1 && x.EquipoId == 2);
        Assert.Equal(0, perdedor.Pts);
        Assert.Equal(1, perdedor.GF);
        Assert.Equal(2, perdedor.GC);
        Assert.Equal(-1, perdedor.Diff);
        // Último entre los 4: por debajo de los 2 equipos que aún no juegan (diff 0 > -1).
        Assert.Equal(4, perdedor.Posicion);
    }

    [Fact]
    public async Task Recalcular_EsIdempotente_NoDuplicaPuntos()
    {
        using var ctx = TestHelpers.NewContext(nameof(Recalcular_EsIdempotente_NoDuplicaPuntos));
        await PrepararPartido1Terminado(ctx);
        ctx.Predicciones.Add(Pred("ana", 2, 1));
        await ctx.SaveChangesAsync();

        await NewService(ctx).RecalcularAsync(TorneoId);
        await NewService(ctx).RecalcularAsync(TorneoId); // segunda corrida no debe duplicar

        var ana = await ctx.Rankings.AsNoTracking().FirstAsync(r => r.Usuario == "ana");
        Assert.Equal(5, ana.Pts);
        Assert.Equal(1, ana.ResultadoExacto);

        var ganador = await ctx.GruposEquipos.AsNoTracking().FirstAsync(x => x.GrupoId == 1 && x.EquipoId == 1);
        Assert.Equal(3, ganador.Pts);
        Assert.Equal(2, ganador.GF);
    }

    [Fact]
    public async Task Recalcular_IgnoraPrediccionesIncompletas()
    {
        using var ctx = TestHelpers.NewContext(nameof(Recalcular_IgnoraPrediccionesIncompletas));
        await PrepararPartido1Terminado(ctx);
        ctx.Predicciones.Add(Pred("ana", 2, 1)); // completa
        ctx.Predicciones.Add(new Prediccion
        {
            QuinielaId = Qid, PartidoId = 1, Team1Resultado = 2, Team2Resultado = null, // incompleta -> no cuenta
            Username = "beto", Active = true, CreatedAt = DateTime.UtcNow, CreatedBy = "test",
        });
        await ctx.SaveChangesAsync();

        await NewService(ctx).RecalcularAsync(TorneoId);

        Assert.True(await ctx.Rankings.AnyAsync(r => r.Usuario == "ana"));
        Assert.False(await ctx.Rankings.AnyAsync(r => r.Usuario == "beto"));
    }
}

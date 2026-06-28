using Quinela.Application.Features.Ranking;
using Quinela.Infrastructure.Repositories;
using RankingEntity = Quinela.Domain.Entities.Ranking;

namespace Quinela.Application.Tests;

public class RankingHandlersTests
{
    private const int Qid = 1;

    private static RankingEntity Seed(string usuario, int pts, int exacto, int atinado, bool active) => new()
    {
        QuinielaId = Qid,
        Usuario = usuario,
        Pts = pts,
        ResultadoExacto = exacto,
        ResultadoAtinado = atinado,
        Active = active,
        CreatedAt = DateTime.UtcNow,
        CreatedBy = "seed"
    };

    [Fact]
    public async Task GetAll_OrdersByPtsThenExacto_AndSkipsInactive()
    {
        using var ctx = TestHelpers.NewContext(nameof(GetAll_OrdersByPtsThenExacto_AndSkipsInactive));
        ctx.Rankings.AddRange(
            Seed("ana", pts: 10, exacto: 2, atinado: 4, active: true),
            Seed("beto", pts: 20, exacto: 1, atinado: 3, active: true),
            Seed("cita", pts: 20, exacto: 5, atinado: 1, active: true),
            Seed("dario", pts: 99, exacto: 9, atinado: 9, active: false));
        ctx.SaveChanges();

        var handler = new GetAllRankingHandler(new Repository<RankingEntity>(ctx));
        var result = await handler.Handle(new GetAllRankingQuery(Qid), CancellationToken.None);

        Assert.True(result.IsSuccess);
        // El inactivo (dario) se excluye; orden: mayor pts, desempate por exacto.
        Assert.Equal(new[] { "cita", "beto", "ana" }, result.Value.Select(x => x.Usuario).ToArray());
    }

    [Fact]
    public async Task GetAll_CincoEmpatadosEnPts_ElDeMasExactosVaPrimero_RestoEmpatado()
    {
        using var ctx = TestHelpers.NewContext(nameof(GetAll_CincoEmpatadosEnPts_ElDeMasExactosVaPrimero_RestoEmpatado));

        // 5 usuarios empatados en puntos (Pts = 5).
        // "ganador" llegó a esos puntos con resultados exactos (2 exactos),
        // los otros 4 los hicieron a puro resultado acertado (5 atinados, 0 exactos),
        // por lo que entre ellos quedan totalmente empatados (mismos Pts/exacto/atinado).
        ctx.Rankings.AddRange(
            Seed("ganador", pts: 5, exacto: 2, atinado: 0, active: true),
            Seed("aaron", pts: 5, exacto: 0, atinado: 5, active: true),
            Seed("bruno", pts: 5, exacto: 0, atinado: 5, active: true),
            Seed("carla", pts: 5, exacto: 0, atinado: 5, active: true),
            Seed("diana", pts: 5, exacto: 0, atinado: 5, active: true));
        ctx.SaveChanges();

        var handler = new GetAllRankingHandler(new Repository<RankingEntity>(ctx));
        var result = await handler.Handle(new GetAllRankingQuery(Qid), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(5, result.Value.Count);

        // El criterio de desempate (mismos Pts) es tener más resultados exactos:
        // "ganador" va de primero (posición 1).
        Assert.Equal("ganador", result.Value[0].Usuario);
        Assert.Equal(1, result.Value[0].Posicion);

        // Los otros 4 quedan empatados: mismos Pts, mismos exactos y mismos atinados,
        // así que comparten la posición 2 (no hay tercero: la siguiente sería la 6).
        var resto = result.Value.Skip(1).ToList();
        Assert.Equal(
            new[] { "aaron", "bruno", "carla", "diana" },
            resto.Select(x => x.Usuario).OrderBy(u => u).ToArray());
        Assert.All(resto, x =>
        {
            Assert.Equal(5, x.Pts);
            Assert.Equal(0, x.ResultadoExacto);
            Assert.Equal(5, x.ResultadoAtinado);
            Assert.Equal(2, x.Posicion);
        });
        Assert.DoesNotContain(result.Value, x => x.Posicion == 3);
    }

    [Fact]
    public async Task GetAll_MismoPtsYExacto_DistintoAtinado_Empatan()
    {
        using var ctx = TestHelpers.NewContext(nameof(GetAll_MismoPtsYExacto_DistintoAtinado_Empatan));
        // ana y beto tienen los mismos Pts y exactos pero distintos atinados:
        // los atinados NO desempatan, así que comparten la posición 1.
        ctx.Rankings.AddRange(
            Seed("ana", pts: 10, exacto: 3, atinado: 5, active: true),
            Seed("beto", pts: 10, exacto: 3, atinado: 0, active: true),
            Seed("cita", pts: 8, exacto: 2, atinado: 9, active: true));
        ctx.SaveChanges();

        var handler = new GetAllRankingHandler(new Repository<RankingEntity>(ctx));
        var result = await handler.Handle(new GetAllRankingQuery(Qid), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value.Single(x => x.Usuario == "ana").Posicion);
        Assert.Equal(1, result.Value.Single(x => x.Usuario == "beto").Posicion);
        Assert.Equal(3, result.Value.Single(x => x.Usuario == "cita").Posicion);
        Assert.DoesNotContain(result.Value, x => x.Posicion == 2);
    }

    [Fact]
    public async Task GetAll_SinEmpates_PosicionesConsecutivas_1_2_3()
    {
        using var ctx = TestHelpers.NewContext(nameof(GetAll_SinEmpates_PosicionesConsecutivas_1_2_3));
        ctx.Rankings.AddRange(
            Seed("ana", pts: 15, exacto: 5, atinado: 0, active: true),
            Seed("beto", pts: 14, exacto: 4, atinado: 2, active: true),
            Seed("cita", pts: 13, exacto: 3, atinado: 4, active: true));
        ctx.SaveChanges();

        var handler = new GetAllRankingHandler(new Repository<RankingEntity>(ctx));
        var result = await handler.Handle(new GetAllRankingQuery(Qid), CancellationToken.None);

        Assert.True(result.IsSuccess);
        // 15 > 14 > 13: sin empate, posiciones 1,2,3 (oro/plata/bronce).
        Assert.Equal(new[] { "ana", "beto", "cita" }, result.Value.Select(x => x.Usuario).ToArray());
        Assert.Equal(new[] { 1, 2, 3 }, result.Value.Select(x => x.Posicion).ToArray());
    }

    [Fact]
    public async Task GetAll_EmpateEnPrimero_TerceroSaltaAPosicion3_NoHaySegundo()
    {
        using var ctx = TestHelpers.NewContext(nameof(GetAll_EmpateEnPrimero_TerceroSaltaAPosicion3_NoHaySegundo));
        // Dos empatados en 15 (mismos exactos/atinados) y uno con 14.
        ctx.Rankings.AddRange(
            Seed("ana", pts: 15, exacto: 4, atinado: 1, active: true),
            Seed("beto", pts: 15, exacto: 4, atinado: 1, active: true),
            Seed("cita", pts: 14, exacto: 3, atinado: 2, active: true));
        ctx.SaveChanges();

        var handler = new GetAllRankingHandler(new Repository<RankingEntity>(ctx));
        var result = await handler.Handle(new GetAllRankingQuery(Qid), CancellationToken.None);

        Assert.True(result.IsSuccess);
        // Los dos primeros comparten la posición 1 (oro); "cita" no puede ser
        // segundo porque hay dos en primero: salta a la posición 3 (bronce).
        var ana = result.Value.Single(x => x.Usuario == "ana");
        var beto = result.Value.Single(x => x.Usuario == "beto");
        var cita = result.Value.Single(x => x.Usuario == "cita");
        Assert.Equal(1, ana.Posicion);
        Assert.Equal(1, beto.Posicion);
        Assert.Equal(3, cita.Posicion);
        Assert.DoesNotContain(result.Value, x => x.Posicion == 2);
    }

    [Fact]
    public async Task GetAll_TresEmpatadosEnTercero_TodosPosicion3()
    {
        using var ctx = TestHelpers.NewContext(nameof(GetAll_TresEmpatadosEnTercero_TodosPosicion3));
        // 25 (1º), 20 (2º) y tres empatados en puntos y exactos (3º).
        ctx.Rankings.AddRange(
            Seed("ana", pts: 25, exacto: 8, atinado: 1, active: true),
            Seed("beto", pts: 20, exacto: 6, atinado: 2, active: true),
            Seed("cita", pts: 18, exacto: 5, atinado: 3, active: true),
            Seed("dani", pts: 18, exacto: 5, atinado: 3, active: true),
            Seed("eva", pts: 18, exacto: 5, atinado: 3, active: true));
        ctx.SaveChanges();

        var handler = new GetAllRankingHandler(new Repository<RankingEntity>(ctx));
        var result = await handler.Handle(new GetAllRankingQuery(Qid), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value.Single(x => x.Usuario == "ana").Posicion);
        Assert.Equal(2, result.Value.Single(x => x.Usuario == "beto").Posicion);
        // Los tres empatados comparten la posición 3 (bronce).
        foreach (var u in new[] { "cita", "dani", "eva" })
            Assert.Equal(3, result.Value.Single(x => x.Usuario == u).Posicion);
    }
}

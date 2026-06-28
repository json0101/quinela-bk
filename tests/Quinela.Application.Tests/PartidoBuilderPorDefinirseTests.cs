using Quinela.Application.Features.Master.Partidos;
using Quinela.Domain.Entities;

namespace Quinela.Application.Tests;

/// <summary>
/// El builder "por definirse" arma un partido de eliminatoria a partir del árbol
/// (de qué partidos depende), y exige que la fase sea eliminatoria.
/// </summary>
public class PartidoBuilderPorDefinirseTests
{
    private static readonly DateTime Fecha = new(2026, 7, 4, 18, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void PorDefinirse_ConFaseEliminatoria_ArmaConElArbol()
    {
        var p = PartidoBuilder
            .PorDefinirse(faseId: 2, torneoId: 1, fechaPartido: Fecha,
                partidoGanadorLocalId: 74, partidoGanadorVisitanteId: 77)
            .ConAuditoria("admin")
            .Build();

        Assert.True(p.PorDefinirse);
        Assert.Equal(2, p.FaseId);
        Assert.Equal(1, p.TorneoId);
        Assert.Equal(74, p.PartidoGanadorLocalId);
        Assert.Equal(77, p.PartidoGanadorVisitanteId);
        Assert.True(p.AplicaDefinicionPenales);
        Assert.Equal('P', p.Estado);
        // Solo habilita esos campos: grupo y equipos quedan sin definir.
        Assert.Null(p.GrupoId);
        Assert.Null(p.EquipoLocalId);
        Assert.Null(p.EquipoVisitanteId);
    }

    [Fact]
    public void PorDefinirse_ConFaseGrupos_Falla()
    {
        Assert.Throws<InvalidOperationException>(() =>
            PartidoBuilder.PorDefinirse(faseId: Fase.GruposId, torneoId: 1, fechaPartido: Fecha,
                partidoGanadorLocalId: 74, partidoGanadorVisitanteId: 77));
    }
}

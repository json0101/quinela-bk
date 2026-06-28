using Quinela.Domain.Entities;

namespace Quinela.Application.Features.Master.Partidos
{
    /// <summary>Campos extra que solo existen en la fase de eliminatoria.</summary>
    public sealed record DefinicionEliminatoria(
        bool AplicaDefinicionPenales,
        bool? SeDefiniraEnPenales,
        int? PenalesAnotadosLocal,
        int? PenalesAnotadosVisitante,
        int? EquipoGanadorId,
        int? PartidoGanadorLocalId,
        int? PartidoGanadorVisitanteId);

    /// <summary>
    /// Builder de partidos. La ficha base se arma igual para cualquier fase
    /// (la lógica de grupos); para eliminatoria se agrega, además, la definición
    /// (penales, equipo ganador y el árbol). En grupos esos campos quedan nulos
    /// y <c>AplicaDefinicionPenales = false</c> por omisión.
    /// </summary>
    public sealed class PartidoBuilder
    {
        private readonly Partido _partido;

        private PartidoBuilder(int faseId) => _partido = new Partido { FaseId = faseId };

        public static PartidoBuilder DeFase(int faseId) => new(faseId);

        /// <summary>
        /// Construye un partido de eliminatoria "por definirse": sus participantes salen
        /// del árbol (ganadores de otros partidos). Solo habilita estos campos: de qué
        /// partidos depende (local/visitante), la fecha y el torneo. La fase DEBE ser
        /// eliminatoria (validación del BK).
        /// </summary>
        public static PartidoBuilder PorDefinirse(int faseId, int torneoId, DateTime fechaPartido,
            int? partidoGanadorLocalId, int? partidoGanadorVisitanteId)
        {
            if (FasesConocidas.Tipo(faseId) != TipoFase.Eliminatoria)
                throw new InvalidOperationException("Un partido 'por definirse' solo puede ser de fase eliminatoria.");

            var b = new PartidoBuilder(faseId);
            b._partido.TorneoId = torneoId;
            b._partido.FechaPartido = DateTime.SpecifyKind(fechaPartido, DateTimeKind.Utc);
            b._partido.PorDefinirse = true;
            b._partido.AplicaDefinicionPenales = true;
            b._partido.PartidoGanadorLocalId = partidoGanadorLocalId;
            b._partido.PartidoGanadorVisitanteId = partidoGanadorVisitanteId;
            b._partido.Estado = 'P';
            return b;
        }

        public PartidoBuilder ConFicha(
            DateTime fechaPartido, int torneoId, int grupoId, int equipoLocalId,
            int equipoVisitanteId, int tipoPartidoId, string? partidoIdApi, bool active)
        {
            _partido.FechaPartido = DateTime.SpecifyKind(fechaPartido, DateTimeKind.Utc);
            _partido.TorneoId = torneoId;
            _partido.GrupoId = grupoId;
            _partido.EquipoLocalId = equipoLocalId;
            _partido.EquipoVisitanteId = equipoVisitanteId;
            _partido.TipoPartidoId = tipoPartidoId;
            _partido.PartidoIdApi = partidoIdApi;
            _partido.Active = active;
            return this;
        }

        public PartidoBuilder ConEstado(char estado, int? resultadoLocal, int? resultadoVisitante, TipoPartido tipo)
        {
            PartidoEstadoHelper.Aplicar(_partido, estado, resultadoLocal, resultadoVisitante, tipo);
            return this;
        }

        public PartidoBuilder ConAuditoria(string createdBy)
        {
            _partido.CreatedAt = DateTime.UtcNow;
            _partido.CreatedBy = createdBy;
            return this;
        }

        /// <summary>Agrega el "pedazo" de eliminatoria que la fase de grupos no tiene.</summary>
        public PartidoBuilder ConDefinicionEliminatoria(DefinicionEliminatoria definicion)
        {
            DefinicionEliminatoriaAplicador.Aplicar(_partido, definicion);
            return this;
        }

        public Partido Build() => _partido;
    }

    /// <summary>
    /// Aplica/limpia los campos de eliminatoria sobre un partido. Lo usa el builder
    /// (alta) y el handler de edición para reescribir la definición según la fase.
    /// </summary>
    internal static class DefinicionEliminatoriaAplicador
    {
        public static void Aplicar(Partido p, DefinicionEliminatoria d)
        {
            p.AplicaDefinicionPenales = d.AplicaDefinicionPenales;
            p.PartidoSeDefiniraEnPenales = d.SeDefiniraEnPenales;
            p.PenalesAnotadosLocal = d.PenalesAnotadosLocal;
            p.PenalesAnotadosVisitante = d.PenalesAnotadosVisitante;
            p.EquipoGanadorId = d.EquipoGanadorId;
            p.PartidoGanadorLocalId = d.PartidoGanadorLocalId;
            p.PartidoGanadorVisitanteId = d.PartidoGanadorVisitanteId;
        }

        /// <summary>Deja un partido sin definición de eliminatoria (estado de fase de grupos).</summary>
        public static void Limpiar(Partido p)
        {
            p.AplicaDefinicionPenales = false;
            p.PartidoSeDefiniraEnPenales = null;
            p.PenalesAnotadosLocal = null;
            p.PenalesAnotadosVisitante = null;
            p.EquipoGanadorId = null;
            p.PartidoGanadorLocalId = null;
            p.PartidoGanadorVisitanteId = null;
        }
    }
}

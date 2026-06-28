using Quinela.Domain.Entities.Commons;

namespace Quinela.Domain.Entities
{
    /// <summary>
    /// Partido del Mundial 2026: enfrentamiento entre un equipo local y uno visitante
    /// dentro de un grupo. Los resultados (goles) y puntos quedan en null hasta que se juega.
    /// Estado: 'P' previa, 'E' en curso, 'T' terminado.
    /// </summary>
    public class Partido : BaseEntity
    {
        public DateTime FechaPartido { get; set; }

        public int TorneoId { get; set; }
        public Torneo Torneo { get; set; } = null!;

        public int GrupoId { get; set; }
        public Grupo Grupo { get; set; } = null!;

        // Fase del partido (Grupos por defecto, o Eliminatoria). Obligatorio.
        public int FaseId { get; set; }
        public Fase Fase { get; set; } = null!;

        public int EquipoLocalId { get; set; }
        public Equipo EquipoLocal { get; set; } = null!;

        public int EquipoVisitanteId { get; set; }
        public Equipo EquipoVisitante { get; set; } = null!;

        // Equipo que ganó el partido (definición de eliminatoria). Null en grupos
        // o mientras no se defina.
        public int? EquipoGanadorId { get; set; }
        public Equipo? EquipoGanador { get; set; }

        // Goles: null mientras el partido no se ha jugado.
        public int? ResultadoLocalId { get; set; }
        public int? ResultadoVisitanteId { get; set; }

        // Puntos del partido para cada equipo (se calculan al terminar).
        public int? PtsLocal { get; set; }
        public int? PtsVisitante { get; set; }

        public int TipoPartidoId { get; set; }
        public TipoPartido TipoPartido { get; set; } = null!;

        // 'P' previa, 'E' en curso, 'T' terminado.
        public char Estado { get; set; }

        public string? PartidoIdApi { get; set; }

        // --- Definición de eliminatoria (todo null/false en fase de grupos) ---

        // ¿El partido aplica definición por penales? false en grupos; true en eliminatoria.
        public bool AplicaDefinicionPenales { get; set; }

        // ¿Se definirá en penales? (criterio del admin; puede quedar sin definir).
        public bool? PartidoSeDefiniraEnPenales { get; set; }

        // Penales anotados por cada equipo (null si no aplica o no se han jugado).
        public int? PenalesAnotadosLocal { get; set; }
        public int? PenalesAnotadosVisitante { get; set; }

        // Árbol de eliminatoria: partidos cuyos ganadores alimentan este partido.
        public int? PartidoGanadorLocalId { get; set; }
        public int? PartidoGanadorVisitanteId { get; set; }
    }
}

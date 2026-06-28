namespace Quinela.Application.Features.Master.Partidos
{
    /// <summary>Partido para la administración (vista de tabla / edición).</summary>
    public class PartidoAdminDto
    {
        public int Id { get; set; }
        public DateTime FechaPartido { get; set; }

        public int TorneoId { get; set; }
        public string Torneo { get; set; } = string.Empty;

        public int GrupoId { get; set; }
        public string Grupo { get; set; } = string.Empty;

        public int FaseId { get; set; }
        public string Fase { get; set; } = string.Empty;

        public int EquipoLocalId { get; set; }
        public string EquipoLocal { get; set; } = string.Empty;

        public int EquipoVisitanteId { get; set; }
        public string EquipoVisitante { get; set; } = string.Empty;

        public int? EquipoGanadorId { get; set; }
        public string? EquipoGanador { get; set; }

        public int TipoPartidoId { get; set; }
        public string TipoPartido { get; set; } = string.Empty;

        public int? ResultadoLocal { get; set; }
        public int? ResultadoVisitante { get; set; }
        public int? PtsLocal { get; set; }
        public int? PtsVisitante { get; set; }

        public char Estado { get; set; }
        // Id del game en el API externo (worldcup26.ir).
        public string? PartidoIdApi { get; set; }

        // Definición de eliminatoria (null/false en grupos).
        public bool AplicaDefinicionPenales { get; set; }
        public bool? PartidoSeDefiniraEnPenales { get; set; }
        public int? PenalesAnotadosLocal { get; set; }
        public int? PenalesAnotadosVisitante { get; set; }
        public int? PartidoGanadorLocalId { get; set; }
        public int? PartidoGanadorVisitanteId { get; set; }

        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class PartidoCreateDto
    {
        public DateTime FechaPartido { get; set; }
        public int TorneoId { get; set; }
        public int GrupoId { get; set; }
        // Fase del partido (Grupos por defecto). En eliminatoria habilita los campos de definición.
        public int FaseId { get; set; } = 1;
        public int EquipoLocalId { get; set; }
        public int EquipoVisitanteId { get; set; }
        public int TipoPartidoId { get; set; }
        public char Estado { get; set; } = 'P';
        public int? ResultadoLocal { get; set; }
        public int? ResultadoVisitante { get; set; }
        public string? PartidoIdApi { get; set; }
        public bool Active { get; set; } = true;

        // Definición de eliminatoria (se ignora en grupos: el backend la guarda nula).
        public bool? PartidoSeDefiniraEnPenales { get; set; }
        public int? PenalesAnotadosLocal { get; set; }
        public int? PenalesAnotadosVisitante { get; set; }
        public int? EquipoGanadorId { get; set; }
        public int? PartidoGanadorLocalId { get; set; }
        public int? PartidoGanadorVisitanteId { get; set; }
    }

    // Edición manual completa: ficha + estado (P/E/T) + goles. Guardar recalcula el ranking.
    public class PartidoUpdateDto
    {
        public DateTime FechaPartido { get; set; }
        public int TorneoId { get; set; }
        public int GrupoId { get; set; }
        public int FaseId { get; set; } = 1;
        public int EquipoLocalId { get; set; }
        public int EquipoVisitanteId { get; set; }
        public int TipoPartidoId { get; set; }
        public char Estado { get; set; } = 'P';
        public int? ResultadoLocal { get; set; }
        public int? ResultadoVisitante { get; set; }
        public string? PartidoIdApi { get; set; }
        public bool Active { get; set; }

        // Definición de eliminatoria (se ignora en grupos: el backend la guarda nula).
        public bool? PartidoSeDefiniraEnPenales { get; set; }
        public int? PenalesAnotadosLocal { get; set; }
        public int? PenalesAnotadosVisitante { get; set; }
        public int? EquipoGanadorId { get; set; }
        public int? PartidoGanadorLocalId { get; set; }
        public int? PartidoGanadorVisitanteId { get; set; }
    }
}

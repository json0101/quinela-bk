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

        public int EquipoLocalId { get; set; }
        public string EquipoLocal { get; set; } = string.Empty;

        public int EquipoVisitanteId { get; set; }
        public string EquipoVisitante { get; set; } = string.Empty;

        public int TipoPartidoId { get; set; }
        public string TipoPartido { get; set; } = string.Empty;

        public int? ResultadoLocal { get; set; }
        public int? ResultadoVisitante { get; set; }
        public int? PtsLocal { get; set; }
        public int? PtsVisitante { get; set; }

        public char Estado { get; set; }
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
        public int EquipoLocalId { get; set; }
        public int EquipoVisitanteId { get; set; }
        public int TipoPartidoId { get; set; }
        public char Estado { get; set; } = 'P';
        public int? ResultadoLocal { get; set; }
        public int? ResultadoVisitante { get; set; }
        public bool Active { get; set; } = true;
    }

    // Edición manual completa: ficha + estado (P/E/T) + goles. Guardar recalcula el ranking.
    public class PartidoUpdateDto
    {
        public DateTime FechaPartido { get; set; }
        public int TorneoId { get; set; }
        public int GrupoId { get; set; }
        public int EquipoLocalId { get; set; }
        public int EquipoVisitanteId { get; set; }
        public int TipoPartidoId { get; set; }
        public char Estado { get; set; } = 'P';
        public int? ResultadoLocal { get; set; }
        public int? ResultadoVisitante { get; set; }
        public bool Active { get; set; }
    }
}

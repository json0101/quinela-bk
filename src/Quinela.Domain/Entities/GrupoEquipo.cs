using Quinela.Domain.Entities.Commons;

namespace Quinela.Domain.Entities
{
    /// <summary>
    /// Tabla intermedia Grupo–Equipo: ubica al equipo en su grupo y lleva su
    /// tabla de posiciones (puntos, goles a favor/contra, diferencia y posición).
    /// Al precargar arranca todo en 0 y la posición es el orden inicial en el grupo.
    /// </summary>
    public class GrupoEquipo : BaseEntity
    {
        // Redundante pero explícito: el grupo y el equipo deben pertenecer a este torneo.
        public int TorneoId { get; set; }
        public Torneo Torneo { get; set; } = null!;

        public int GrupoId { get; set; }
        public Grupo Grupo { get; set; } = null!;

        public int EquipoId { get; set; }
        public Equipo Equipo { get; set; } = null!;

        public int Pts { get; set; }   // Puntos
        public int GF { get; set; }    // Goles a favor
        public int GC { get; set; }    // Goles en contra
        public int Diff { get; set; }  // Diferencia de goles
        public int Posicion { get; set; } // Posición dentro del grupo (1..4)
    }
}

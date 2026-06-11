using Quinela.Domain.Entities.Commons;

namespace Quinela.Domain.Entities
{
    /// <summary>Grupo del Mundial 2026 (A..L).</summary>
    public class Grupo : BaseEntity
    {
        public string Nombre { get; set; } = null!;

        public int TorneoId { get; set; }
        public Torneo Torneo { get; set; } = null!;

        // Equipos del grupo (con su posición y estadísticas) vía tabla intermedia.
        public ICollection<GrupoEquipo> GrupoEquipos { get; set; } = new List<GrupoEquipo>();
    }
}

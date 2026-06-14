using Quinela.Domain.Entities.Commons;

namespace Quinela.Domain.Entities
{
    /// <summary>Selección participante del Mundial 2026.</summary>
    public class Equipo : BaseEntity
    {
        public string Nombre { get; set; } = null!;
        public string Confederacion { get; set; } = null!;
        public bool Anfitrion { get; set; }

        public int TorneoId { get; set; }
        public Torneo Torneo { get; set; } = null!;

        // Nombre del archivo de la bandera (ej. "mx.svg"). La imagen vive en el FT: public/img/flags.
        public string? UrlBandera { get; set; }

        public string? EquipoIdApi { get; set; }
        public string? EquipoIdApiLargo { get; set; }

        // Relación con su grupo a través de la tabla intermedia (posición + estadísticas).
        public ICollection<GrupoEquipo> GrupoEquipos { get; set; } = new List<GrupoEquipo>();
    }
}

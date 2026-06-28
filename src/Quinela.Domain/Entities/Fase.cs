using Quinela.Domain.Entities.Commons;

namespace Quinela.Domain.Entities
{
    /// <summary>Fase de un torneo (ej. Grupos, Eliminatoria).</summary>
    public class Fase : BaseEntity
    {
        /// <summary>Id sembrado de la fase "Grupos" (la fase por defecto de un partido).</summary>
        public const int GruposId = 1;

        public string Descripcion { get; set; } = null!;
        public int TorneoId { get; set; }
        public Torneo Torneo { get; set; } = null!;
    }
}

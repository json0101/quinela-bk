using Quinela.Domain.Entities.Commons;

namespace Quinela.Domain.Entities
{
    /// <summary>Torneo (ej. Copa Mundial de la FIFA 2026). Agrupa equipos, grupos y partidos.</summary>
    public class Torneo : BaseEntity
    {
        public string Descripcion { get; set; } = null!;
    }
}

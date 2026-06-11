using Quinela.Domain.Entities.Commons;

namespace Quinela.Domain.Entities
{
    /// <summary>
    /// Quiniela creada a partir de un torneo. Cada torneo puede tener varias quinielas;
    /// las predicciones y el ranking son por quiniela.
    /// </summary>
    public class Quiniela : BaseEntity
    {
        public string Nombre { get; set; } = null!;
        public string Reglas { get; set; } = null!;

        public int TorneoId { get; set; }
        public Torneo Torneo { get; set; } = null!;
    }
}

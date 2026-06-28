using Quinela.Domain.Entities.Commons;

namespace Quinela.Domain.Entities
{
    /// <summary>
    /// Configuración de puntajes según el tipo de partido (ej. fase de grupos, final):
    /// puntos del partido (victoria/empate) y puntos de la quiniela (resultado exacto/acertado).
    /// </summary>
    public class TipoPartido : BaseEntity
    {
        public string Descripcion { get; set; } = null!;
        public int FaseId { get; set; }
        public Fase Fase { get; set; } = null!;
        public int PtsPartidoVictoria { get; set; }
        public int PtsPartidoEmpate { get; set; }
        public int PtsQuinelaResultadoExacto { get; set; }
        public int PtsQuinelaResultadoAcertado { get; set; }
    }
}

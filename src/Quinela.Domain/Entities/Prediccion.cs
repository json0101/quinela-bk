using Quinela.Domain.Entities.Commons;

namespace Quinela.Domain.Entities
{
    /// <summary>
    /// Predicción de un usuario para un partido: el marcador que apuesta para cada equipo.
    /// El usuario proviene de UserApp; se valida manualmente que exista y se guarda como
    /// texto (sin llave foránea, ya que vive en otra base/esquema).
    /// </summary>
    public class Prediccion : BaseEntity
    {
        // Las predicciones son por quiniela.
        public int QuinielaId { get; set; }
        public Quiniela Quiniela { get; set; } = null!;

        public int PartidoId { get; set; }
        public Partido Partido { get; set; } = null!;

        // Nullable: el usuario puede guardar primero el del local y dejar el otro en null.
        public int? Team1Resultado { get; set; }
        public int? Team2Resultado { get; set; }

        // Username del autor (validado contra UserApp, guardado como texto, sin FK).
        public string Username { get; set; } = null!;
    }
}

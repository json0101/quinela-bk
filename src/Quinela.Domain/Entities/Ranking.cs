using Quinela.Domain.Entities.Commons;

namespace Quinela.Domain.Entities
{
    /// <summary>
    /// Posición de un usuario en la tabla general de la quiniela. Se calcula por lógica
    /// (no se edita por endpoint). El usuario proviene de UserApp y se guarda como texto.
    /// </summary>
    public class Ranking : BaseEntity
    {
        // Username del usuario (texto, sin FK; proviene de UserApp).
        public string Usuario { get; set; } = null!;

        public int Pts { get; set; }
        public int ResultadoAtinado { get; set; }
        public int ResultadoExacto { get; set; }
    }
}

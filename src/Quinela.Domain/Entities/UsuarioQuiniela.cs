using Quinela.Domain.Entities.Commons;

namespace Quinela.Domain.Entities
{
    /// <summary>
    /// Acceso de un usuario (de UserApp, base 'userapp') a una quiniela (base 'quinela').
    /// La tabla vive en la base 'quinela'. Guarda el user_id de sec.users; NO hay FK a
    /// users porque está en otra base de datos. La quiniela sí es FK local.
    /// </summary>
    public class UsuarioQuiniela : BaseEntity
    {
        public int UserId { get; set; }

        public int QuinielaId { get; set; }
        public Quiniela Quiniela { get; set; } = null!;
    }
}

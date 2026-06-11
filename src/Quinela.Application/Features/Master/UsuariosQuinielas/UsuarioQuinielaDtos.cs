namespace Quinela.Application.Features.Master.UsuariosQuinielas
{
    // Fila de acceso usuario↔quiniela (para la tabla del CRUD).
    public class UsuarioQuinielaDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int QuinielaId { get; set; }
        public string Quiniela { get; set; } = string.Empty;
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }

    // Usuario de UserApp (sec.users) para el combo del formulario.
    public class UsuarioAppDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class UsuarioQuinielaCreateDto
    {
        public int UserId { get; set; }
        public int QuinielaId { get; set; }
        public bool Active { get; set; } = true;
    }

    public class UsuarioQuinielaUpdateDto
    {
        public int UserId { get; set; }
        public int QuinielaId { get; set; }
        public bool Active { get; set; }
    }
}

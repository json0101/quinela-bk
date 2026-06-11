namespace Quinela.Application.Common.Abstractions
{
    public interface ICurrentUser
    {
        string UserName { get; }

        // user_id de sec.users (del claim Sid del JWT). null si no hay sesión.
        int? UserId { get; }

        // Datos de la petición HTTP actual (para auditoría/bitácora). null fuera de un request.
        string? IpAddress { get; }
        string? UserAgent { get; }
    }
}

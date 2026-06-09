using Quinela.Application.Common.Results;

namespace Quinela.Application.Features.Auth.Login
{
    public static class LoginErrors
    {
        public static readonly Error UsernameRequired = Error.Validation("Login.Username", "El nombre de usuario es requerido.");
        public static readonly Error UserNotFound = Error.NotFound("Login.User", "No se encontro el usuario");
        // Mismo mensaje generico para usuario inexistente o clave invalida (evita enumeracion de usuarios).
        public static readonly Error InvalidCredentials = Error.Validation("Login.Credentials", "Usuario o contraseña incorrectos");
    }
}

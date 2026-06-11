using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Quinela.Application.Common.Abstractions;
using Quinela.Application.Common.Results;
using Quinela.Application.Commons;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UserApp.Service.Services.Users;
using UserApp.Service.Services.UsersLoginLogs;
using UserApp.Service.Services.UsersLoginLogs.Dtos;

namespace Quinela.Application.Features.Auth.Login
{
    public sealed record LoginCommand(string Username, string Password) : IRequest<Result<string>>;

    public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("El nombre de usuario es requerido.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("La contraseña es requerida.");
        }
    }

    internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, Result<string>>
    {
        private readonly IOptions<AppSetting> _options;
        private readonly IUserService _userService;
        private readonly IUserLoginLogService _loginLogService;
        private readonly ICurrentUser _currentUser;
        private static readonly PasswordHasher<object> _passwordHasher = new();

        // App "Quinela" del seed de UserApp (application_id = 2); igual que Program.ConfigureService.
        private const int QuinelaApplicationId = 2;

        public LoginCommandHandler(IOptions<AppSetting> options, IUserService userService,
            IUserLoginLogService loginLogService, ICurrentUser currentUser)
        {
            _options = options;
            _userService = userService;
            _loginLogService = loginLogService;
            _currentUser = currentUser;
        }

        public Task<Result<string>> Handle(LoginCommand cmd, CancellationToken ct)
        {
            var userdb = _userService.GetUserByEmail(cmd.Username);
            if (userdb is null || string.IsNullOrEmpty(userdb.Password))
            {
                RegistrarLogin(null, cmd.Username, false, "Usuario no encontrado");
                return Task.FromResult(Result.Failure<string>(LoginErrors.InvalidCredentials));
            }

            // Verificar la contraseña contra el hash PBKDF2 (+ salt) guardado por UserApp.
            var verification = _passwordHasher.VerifyHashedPassword(this, userdb.Password, cmd.Password ?? string.Empty);
            if (verification == PasswordVerificationResult.Failed)
            {
                RegistrarLogin(userdb.Id, userdb.UserName, false, "Contraseña incorrecta");
                return Task.FromResult(Result.Failure<string>(LoginErrors.InvalidCredentials));
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Sid, userdb.Id.ToString()),
                new(ClaimTypes.Name, userdb.UserName)
            };

            var jwtToken = new JwtSecurityToken(
                issuer: _options.Value.JwtIssuer,
                audience: _options.Value.JwtAudience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.JwtSecret ?? string.Empty)),
                    SecurityAlgorithms.HmacSha256Signature));

            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            RegistrarLogin(userdb.Id, userdb.UserName, true, null);
            return Task.FromResult(Result.Success(token));
        }

        // Bitácora de inicios de sesión (sec.users_login_logs vía UserApp.Service).
        // Nunca debe tumbar el login: si falla el registro, se ignora.
        private void RegistrarLogin(int? userId, string userName, bool successful, string? failureReason)
        {
            try
            {
                _loginLogService.RegisterLogin(new CreateUserLoginLogDto(
                    userId,
                    QuinelaApplicationId,
                    userName ?? string.Empty,
                    successful,
                    _currentUser.IpAddress ?? string.Empty,
                    _currentUser.UserAgent ?? string.Empty,
                    failureReason ?? string.Empty));
            }
            catch
            {
                // El registro de bitácora es best-effort; no interrumpe la autenticación.
            }
        }
    }
}

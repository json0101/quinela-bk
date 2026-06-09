using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Quinela.Application.Common.Results;
using Quinela.Application.Commons;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UserApp.Service.Services.Users;

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
        private static readonly PasswordHasher<object> _passwordHasher = new();

        public LoginCommandHandler(IOptions<AppSetting> options, IUserService userService)
        {
            _options = options;
            _userService = userService;
        }

        public Task<Result<string>> Handle(LoginCommand cmd, CancellationToken ct)
        {
            var userdb = _userService.GetUserByEmail(cmd.Username);
            if (userdb is null || string.IsNullOrEmpty(userdb.Password))
                return Task.FromResult(Result.Failure<string>(LoginErrors.InvalidCredentials));

            // Verificar la contraseña contra el hash PBKDF2 (+ salt) guardado por UserApp.
            var verification = _passwordHasher.VerifyHashedPassword(this, userdb.Password, cmd.Password ?? string.Empty);
            if (verification == PasswordVerificationResult.Failed)
                return Task.FromResult(Result.Failure<string>(LoginErrors.InvalidCredentials));

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
            return Task.FromResult(Result.Success(token));
        }
    }
}

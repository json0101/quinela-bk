using System.Security.Claims;
using Quinela.Application.Common.Abstractions;

namespace Quinela.Api.Common
{
    public sealed class CurrentUserService : ICurrentUser
    {
        private readonly IHttpContextAccessor _accessor;

        public CurrentUserService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string UserName
        {
            get
            {
                var name = _accessor.HttpContext?.User?.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                return string.IsNullOrWhiteSpace(name) ? "system" : name;
            }
        }

        // user_id del JWT (claim Sid). El login lo emite como ClaimTypes.Sid = userdb.Id.
        public int? UserId
        {
            get
            {
                var sid = _accessor.HttpContext?.User?.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
                return int.TryParse(sid, out var id) ? id : null;
            }
        }

        // IP real del cliente: tras UseForwardedHeaders refleja X-Forwarded-For (detrás de Caddy).
        public string? IpAddress => _accessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

        public string? UserAgent
        {
            get
            {
                var ua = _accessor.HttpContext?.Request.Headers.UserAgent.ToString();
                return string.IsNullOrWhiteSpace(ua) ? null : ua;
            }
        }
    }
}

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
    }
}

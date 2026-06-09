using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quinela.Api.Common;
using Quinela.Application.Features.Auth.Menu;

namespace Quinela.Api.Controllers.Auth
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly ISender _sender;

        public MenuController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> GetMenu(CancellationToken ct)
        {
            var idClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
            if (idClaim is null || !int.TryParse(idClaim.Value, out var userId))
            {
                return Unauthorized(new { message = "Token sin identificador de usuario." });
            }

            return (await _sender.Send(new GetUserMenuQuery(userId), ct)).ToActionResult();
        }
    }
}

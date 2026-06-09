using Quinela.Api.Common;
using Quinela.Application.Features.Auth.Login;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Quinela.Api.Controllers.Auth
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ISender _sender;

        public AuthenticationController(ISender sender)
        {
            _sender = sender;
        }

        [AllowAnonymous]
        [HttpPost]
        [EnableRateLimiting("login")]
        public async Task<IActionResult> Login([FromBody] AuthDto dto, CancellationToken ct)
            => (await _sender.Send(new LoginCommand(dto.Username, dto.Password), ct)).ToActionResult();
    }
}

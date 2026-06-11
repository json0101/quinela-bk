using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quinela.Api.Common;
using Quinela.Application.Features.Quinielas;

namespace Quinela.Api.Controllers
{
    /// <summary>Quinielas desde la óptica del usuario autenticado (no es el master/admin).</summary>
    [Authorize]
    [ApiController]
    [Route("quinielas")]
    public class QuinielasController : ControllerBase
    {
        private readonly ISender _sender;
        public QuinielasController(ISender sender) => _sender = sender;

        // Quinielas a las que el usuario logueado tiene acceso (para los selectores).
        [HttpGet("mias")]
        public async Task<IActionResult> Mias(CancellationToken ct)
            => (await _sender.Send(new GetMisQuinielasQuery(), ct)).ToActionResult();
    }
}

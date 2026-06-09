using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quinela.Api.Common;
using Quinela.Application.Features.Grupos;

namespace Quinela.Api.Controllers
{
    /// <summary>Vista de grupos con su tabla de posiciones (read-only).</summary>
    [Authorize]
    [ApiController]
    [Route("grupos")]
    public class GruposController : ControllerBase
    {
        private readonly ISender _sender;
        public GruposController(ISender sender) => _sender = sender;

        [HttpGet]
        public async Task<IActionResult> GetTabla(CancellationToken ct)
            => (await _sender.Send(new GetGruposTablaQuery(), ct)).ToActionResult();
    }
}

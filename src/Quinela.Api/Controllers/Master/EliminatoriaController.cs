using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quinela.Application.Features.Eliminatoria;

namespace Quinela.Api.Controllers.Master
{
    /// <summary>
    /// Cuadro de eliminatoria del Mundial 2026: preview (solo lectura) y recálculo
    /// (resuelve equipos según las posiciones de grupo y propaga el árbol).
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("master/[controller]")]
    public class EliminatoriaController : ControllerBase
    {
        private readonly IDistributionEliminatoryWorldCup2026 _service;
        public EliminatoriaController(IDistributionEliminatoryWorldCup2026 service) => _service = service;

        [HttpGet("preview")]
        public async Task<IActionResult> Preview([FromQuery] int torneoId, CancellationToken ct)
            => Ok(await _service.PreviewAsync(torneoId == 0 ? 1 : torneoId, ct));

        [HttpPost("recalcular")]
        public async Task<IActionResult> Recalcular([FromQuery] int torneoId, CancellationToken ct)
            => Ok(await _service.RecalcularAsync(torneoId == 0 ? 1 : torneoId, ct));
    }
}

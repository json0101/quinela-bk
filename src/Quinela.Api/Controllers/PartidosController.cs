using System;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quinela.Api.Common;
using Quinela.Application.Features.Partidos;

namespace Quinela.Api.Controllers
{
    /// <summary>Calendario de partidos (read-only) con filtro por rango de fecha.</summary>
    [Authorize]
    [ApiController]
    [Route("partidos")]
    public class PartidosController : ControllerBase
    {
        private readonly ISender _sender;
        public PartidosController(ISender sender) => _sender = sender;

        [HttpGet]
        public async Task<IActionResult> GetCalendario(
            [FromQuery] DateTime? desde, [FromQuery] DateTime? hasta, CancellationToken ct)
            => (await _sender.Send(new GetPartidosCalendarioQuery(desde, hasta), ct)).ToActionResult();
    }
}

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

        // Cambia el estado del partido (P->E->T) y dispara el recálculo de grupos y ranking.
        [HttpPut("{id:int}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] CambiarEstadoPartidoDto dto, CancellationToken ct)
            => (await _sender.Send(
                new CambiarEstadoPartidoCommand(id, dto.Estado, dto.ResultadoLocal, dto.ResultadoVisitante), ct))
                .ToActionResult();
    }

    public class CambiarEstadoPartidoDto
    {
        public char Estado { get; set; }
        public int? ResultadoLocal { get; set; }
        public int? ResultadoVisitante { get; set; }
    }
}

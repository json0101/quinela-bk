using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quinela.Api.Common;
using Quinela.Application.Features.Master.Partidos;

namespace Quinela.Api.Controllers.Master
{
    /// <summary>Administración (CRUD) de partidos: alta, edición de la ficha y baja lógica.</summary>
    [Authorize]
    [ApiController]
    [Route("master/[controller]")]
    public class PartidoController : ControllerBase
    {
        private readonly ISender _sender;
        public PartidoController(ISender sender) => _sender = sender;

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? torneoId, CancellationToken ct)
            => (await _sender.Send(new GetAllPartidosQuery(torneoId), ct)).ToActionResult();

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
            => (await _sender.Send(new GetPartidoByIdQuery(id), ct)).ToActionResult();

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PartidoCreateDto dto, CancellationToken ct)
            => (await _sender.Send(new CreatePartidoCommand(
                dto.FechaPartido, dto.TorneoId, dto.GrupoId, dto.EquipoLocalId, dto.EquipoVisitanteId, dto.TipoPartidoId,
                dto.Estado, dto.ResultadoLocal, dto.ResultadoVisitante, dto.Active), ct)).ToActionResult();

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] PartidoUpdateDto dto, CancellationToken ct)
            => (await _sender.Send(new UpdatePartidoCommand(
                id, dto.FechaPartido, dto.TorneoId, dto.GrupoId, dto.EquipoLocalId, dto.EquipoVisitanteId, dto.TipoPartidoId,
                dto.Estado, dto.ResultadoLocal, dto.ResultadoVisitante, dto.Active), ct)).ToActionResult();

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
            => (await _sender.Send(new DeletePartidoCommand(id), ct)).ToNoContentResult();
    }
}

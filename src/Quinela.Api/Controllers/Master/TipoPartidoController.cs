using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quinela.Api.Common;
using Quinela.Application.Features.Master.TiposPartido;

namespace Quinela.Api.Controllers.Master
{
    [Authorize]
    [ApiController]
    [Route("master/[controller]")]
    public class TipoPartidoController : ControllerBase
    {
        private readonly ISender _sender;
        public TipoPartidoController(ISender sender) => _sender = sender;

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
            => (await _sender.Send(new GetAllTiposPartidoQuery(), ct)).ToActionResult();

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
            => (await _sender.Send(new GetTipoPartidoByIdQuery(id), ct)).ToActionResult();

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TipoPartidoCreateDto dto, CancellationToken ct)
            => (await _sender.Send(new CreateTipoPartidoCommand(
                dto.Descripcion, dto.PtsPartidoVictoria, dto.PtsPartidoEmpate,
                dto.PtsQuinelaResultadoExacto, dto.PtsQuinelaResultadoAcertado, dto.Active), ct)).ToActionResult();

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] TipoPartidoUpdateDto dto, CancellationToken ct)
            => (await _sender.Send(new UpdateTipoPartidoCommand(
                id, dto.Descripcion, dto.PtsPartidoVictoria, dto.PtsPartidoEmpate,
                dto.PtsQuinelaResultadoExacto, dto.PtsQuinelaResultadoAcertado, dto.Active), ct)).ToActionResult();

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
            => (await _sender.Send(new DeleteTipoPartidoCommand(id), ct)).ToNoContentResult();
    }
}

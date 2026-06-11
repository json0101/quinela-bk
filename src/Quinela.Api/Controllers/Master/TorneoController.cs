using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quinela.Api.Common;
using Quinela.Application.Features.Master.Torneos;

namespace Quinela.Api.Controllers.Master
{
    [Authorize]
    [ApiController]
    [Route("master/[controller]")]
    public class TorneoController : ControllerBase
    {
        private readonly ISender _sender;
        public TorneoController(ISender sender) => _sender = sender;

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
            => (await _sender.Send(new GetAllTorneosQuery(), ct)).ToActionResult();

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TorneoCreateDto dto, CancellationToken ct)
            => (await _sender.Send(new CreateTorneoCommand(dto.Descripcion, dto.Active), ct)).ToActionResult();

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] TorneoUpdateDto dto, CancellationToken ct)
            => (await _sender.Send(new UpdateTorneoCommand(id, dto.Descripcion, dto.Active), ct)).ToActionResult();

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
            => (await _sender.Send(new DeleteTorneoCommand(id), ct)).ToNoContentResult();
    }
}

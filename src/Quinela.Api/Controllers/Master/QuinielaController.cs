using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quinela.Api.Common;
using Quinela.Application.Features.Master.Quinielas;

namespace Quinela.Api.Controllers.Master
{
    [Authorize]
    [ApiController]
    [Route("master/[controller]")]
    public class QuinielaController : ControllerBase
    {
        private readonly ISender _sender;
        public QuinielaController(ISender sender) => _sender = sender;

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
            => (await _sender.Send(new GetAllQuinielasQuery(), ct)).ToActionResult();

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
            => (await _sender.Send(new GetQuinielaByIdQuery(id), ct)).ToActionResult();

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] QuinielaCreateDto dto, CancellationToken ct)
            => (await _sender.Send(new CreateQuinielaCommand(dto.Nombre, dto.Reglas, dto.TorneoId, dto.Active), ct)).ToActionResult();

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] QuinielaUpdateDto dto, CancellationToken ct)
            => (await _sender.Send(new UpdateQuinielaCommand(id, dto.Nombre, dto.Reglas, dto.TorneoId, dto.Active), ct)).ToActionResult();

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
            => (await _sender.Send(new DeleteQuinielaCommand(id), ct)).ToNoContentResult();
    }
}

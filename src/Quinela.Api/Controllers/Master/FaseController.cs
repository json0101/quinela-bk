using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quinela.Api.Common;
using Quinela.Application.Features.Master.Fases;

namespace Quinela.Api.Controllers.Master
{
    [Authorize]
    [ApiController]
    [Route("master/[controller]")]
    public class FaseController : ControllerBase
    {
        private readonly ISender _sender;
        public FaseController(ISender sender) => _sender = sender;

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
            => (await _sender.Send(new GetAllFasesQuery(), ct)).ToActionResult();

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
            => (await _sender.Send(new GetFaseByIdQuery(id), ct)).ToActionResult();

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FaseCreateDto dto, CancellationToken ct)
            => (await _sender.Send(new CreateFaseCommand(dto.Descripcion, dto.TorneoId, dto.Active), ct)).ToActionResult();

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] FaseUpdateDto dto, CancellationToken ct)
            => (await _sender.Send(new UpdateFaseCommand(id, dto.Descripcion, dto.TorneoId, dto.Active), ct)).ToActionResult();

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
            => (await _sender.Send(new DeleteFaseCommand(id), ct)).ToNoContentResult();
    }
}

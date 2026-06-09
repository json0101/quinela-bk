using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quinela.Api.Common;
using Quinela.Application.Features.Master.Equipos;

namespace Quinela.Api.Controllers.Master
{
    [Authorize]
    [ApiController]
    [Route("master/[controller]")]
    public class EquipoController : ControllerBase
    {
        private readonly ISender _sender;
        public EquipoController(ISender sender) => _sender = sender;

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
            => (await _sender.Send(new GetAllEquiposQuery(), ct)).ToActionResult();

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
            => (await _sender.Send(new GetEquipoByIdQuery(id), ct)).ToActionResult();

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EquipoCreateDto dto, CancellationToken ct)
            => (await _sender.Send(new CreateEquipoCommand(dto.Nombre, dto.Confederacion, dto.Anfitrion, dto.UrlBandera, dto.Active), ct)).ToActionResult();

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] EquipoUpdateDto dto, CancellationToken ct)
            => (await _sender.Send(new UpdateEquipoCommand(id, dto.Nombre, dto.Confederacion, dto.Anfitrion, dto.UrlBandera, dto.Active), ct)).ToActionResult();

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
            => (await _sender.Send(new DeleteEquipoCommand(id), ct)).ToNoContentResult();
    }
}

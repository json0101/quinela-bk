using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quinela.Api.Common;
using Quinela.Application.Features.Master.UsuariosQuinielas;

namespace Quinela.Api.Controllers.Master
{
    /// <summary>CRUD de accesos usuario↔quiniela. La tabla vive en la base 'quinela'.</summary>
    [Authorize]
    [ApiController]
    [Route("master/[controller]")]
    public class UsuarioQuinielaController : ControllerBase
    {
        private readonly ISender _sender;
        public UsuarioQuinielaController(ISender sender) => _sender = sender;

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
            => (await _sender.Send(new GetAllUsuariosQuinielasQuery(), ct)).ToActionResult();

        // Usuarios de UserApp (sec.users) para el combo del formulario.
        [HttpGet("usuarios")]
        public async Task<IActionResult> GetUsuarios(CancellationToken ct)
            => (await _sender.Send(new GetUsuariosAppQuery(), ct)).ToActionResult();

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UsuarioQuinielaCreateDto dto, CancellationToken ct)
            => (await _sender.Send(new CreateUsuarioQuinielaCommand(dto.UserId, dto.QuinielaId, dto.Active), ct)).ToActionResult();

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UsuarioQuinielaUpdateDto dto, CancellationToken ct)
            => (await _sender.Send(new UpdateUsuarioQuinielaCommand(id, dto.UserId, dto.QuinielaId, dto.Active), ct)).ToActionResult();

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
            => (await _sender.Send(new DeleteUsuarioQuinielaCommand(id), ct)).ToNoContentResult();
    }
}

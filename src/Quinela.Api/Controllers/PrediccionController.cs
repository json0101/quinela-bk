using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quinela.Api.Common;
using Quinela.Application.Features.Predicciones;

namespace Quinela.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PrediccionController : ControllerBase
    {
        private readonly ISender _sender;
        public PrediccionController(ISender sender) => _sender = sender;

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int quinielaId, CancellationToken ct)
            => (await _sender.Send(new GetAllPrediccionesQuery(quinielaId), ct)).ToActionResult();

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
            => (await _sender.Send(new GetPrediccionByIdQuery(id), ct)).ToActionResult();

        // En vivo: todas las predicciones de TODOS los usuarios para un partido en una quiniela.
        [HttpGet("live")]
        public async Task<IActionResult> GetLive([FromQuery] int quinielaId, [FromQuery] int partidoId, CancellationToken ct)
            => (await _sender.Send(new GetPrediccionesLiveQuery(quinielaId, partidoId), ct)).ToActionResult();

        // Predicciones de un usuario en una quiniela, solo de partidos terminados (con puntos).
        [HttpGet("usuario")]
        public async Task<IActionResult> GetDeUsuario([FromQuery] int quinielaId, [FromQuery] string username, CancellationToken ct)
            => (await _sender.Send(new GetPrediccionesUsuarioQuery(quinielaId, username ?? string.Empty), ct)).ToActionResult();

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PrediccionCreateDto dto, CancellationToken ct)
            => (await _sender.Send(new CreatePrediccionCommand(dto.QuinielaId, dto.PartidoId, dto.Team1Resultado, dto.Team2Resultado, dto.Active), ct)).ToActionResult();

        // Crea o actualiza la predicción del usuario autenticado para un partido en una quiniela (solo si está en 'P').
        [HttpPost("upsert")]
        public async Task<IActionResult> Upsert([FromBody] PrediccionUpsertDto dto, CancellationToken ct)
            => (await _sender.Send(new UpsertPrediccionCommand(dto.QuinielaId, dto.PartidoId, dto.Team1Resultado, dto.Team2Resultado), ct)).ToActionResult();

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] PrediccionUpdateDto dto, CancellationToken ct)
            => (await _sender.Send(new UpdatePrediccionCommand(id, dto.QuinielaId, dto.PartidoId, dto.Team1Resultado, dto.Team2Resultado, dto.Active), ct)).ToActionResult();

        // Soft delete: inactiva la predicción.
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
            => (await _sender.Send(new DeletePrediccionCommand(id), ct)).ToNoContentResult();
    }
}

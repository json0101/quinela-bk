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
        public async Task<IActionResult> GetAll(CancellationToken ct)
            => (await _sender.Send(new GetAllPrediccionesQuery(), ct)).ToActionResult();

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
            => (await _sender.Send(new GetPrediccionByIdQuery(id), ct)).ToActionResult();

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PrediccionCreateDto dto, CancellationToken ct)
            => (await _sender.Send(new CreatePrediccionCommand(dto.PartidoId, dto.Team1Resultado, dto.Team2Resultado, dto.Active), ct)).ToActionResult();

        // Crea o actualiza la predicción del usuario autenticado para un partido (solo si está en 'P').
        [HttpPost("upsert")]
        public async Task<IActionResult> Upsert([FromBody] PrediccionUpsertDto dto, CancellationToken ct)
            => (await _sender.Send(new UpsertPrediccionCommand(dto.PartidoId, dto.Team1Resultado, dto.Team2Resultado), ct)).ToActionResult();

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] PrediccionUpdateDto dto, CancellationToken ct)
            => (await _sender.Send(new UpdatePrediccionCommand(id, dto.PartidoId, dto.Team1Resultado, dto.Team2Resultado, dto.Active), ct)).ToActionResult();

        // Soft delete: inactiva la predicción.
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
            => (await _sender.Send(new DeletePrediccionCommand(id), ct)).ToNoContentResult();
    }
}

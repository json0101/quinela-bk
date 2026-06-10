using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quinela.Api.Common;
using Quinela.Application.Features.Ranking;

namespace Quinela.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class RankingController : ControllerBase
    {
        private readonly ISender _sender;
        public RankingController(ISender sender) => _sender = sender;

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
            => (await _sender.Send(new GetAllRankingQuery(), ct)).ToActionResult();
    }
}

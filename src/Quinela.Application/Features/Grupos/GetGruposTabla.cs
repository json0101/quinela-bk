using MediatR;
using Microsoft.EntityFrameworkCore;
using Quinela.Application.Common.Abstractions;
using Quinela.Application.Common.Results;
using Quinela.Domain.Entities;

namespace Quinela.Application.Features.Grupos
{
    public sealed record GetGruposTablaQuery() : IRequest<Result<List<GrupoTablaDto>>>;

    internal sealed class GetGruposTablaHandler : IRequestHandler<GetGruposTablaQuery, Result<List<GrupoTablaDto>>>
    {
        private readonly IRepository<Grupo> _repo;
        public GetGruposTablaHandler(IRepository<Grupo> repo) => _repo = repo;

        public async Task<Result<List<GrupoTablaDto>>> Handle(GetGruposTablaQuery request, CancellationToken ct)
        {
            var data = await _repo.GetDbSet().AsNoTracking()
                .OrderBy(g => g.Nombre)
                .Select(g => new GrupoTablaDto
                {
                    Id = g.Id,
                    Nombre = g.Nombre,
                    Equipos = g.GrupoEquipos
                        .OrderBy(ge => ge.Posicion)
                        .Select(ge => new GrupoEquipoTablaDto
                        {
                            EquipoId = ge.EquipoId,
                            Equipo = ge.Equipo!.Nombre,
                            UrlBandera = ge.Equipo.UrlBandera,
                            Posicion = ge.Posicion,
                            Pts = ge.Pts,
                            GF = ge.GF,
                            GC = ge.GC,
                            Diff = ge.Diff
                        }).ToList()
                }).ToListAsync(ct);

            return Result.Success(data);
        }
    }
}

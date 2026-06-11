using MediatR;
using Microsoft.EntityFrameworkCore;
using Quinela.Application.Common.Abstractions;
using Quinela.Application.Common.Results;
using Quinela.Domain.Entities;

namespace Quinela.Application.Features.Quinielas
{
    /// <summary>Una quiniela a la que el usuario autenticado tiene acceso (para los selectores).</summary>
    public class MiQuinielaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    // Quinielas a las que el usuario logueado tiene acceso (vía usuarios_quinielas + user_id del token).
    public sealed record GetMisQuinielasQuery() : IRequest<Result<List<MiQuinielaDto>>>;

    internal sealed class GetMisQuinielasHandler : IRequestHandler<GetMisQuinielasQuery, Result<List<MiQuinielaDto>>>
    {
        private readonly IRepository<UsuarioQuiniela> _accesos;
        private readonly ICurrentUser _currentUser;
        public GetMisQuinielasHandler(IRepository<UsuarioQuiniela> accesos, ICurrentUser currentUser)
        { _accesos = accesos; _currentUser = currentUser; }

        public async Task<Result<List<MiQuinielaDto>>> Handle(GetMisQuinielasQuery request, CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            if (userId is null) return Result.Success(new List<MiQuinielaDto>());

            var rows = await _accesos.GetDbSet().AsNoTracking()
                .Where(a => a.Active && a.UserId == userId.Value && a.Quiniela.Active)
                .OrderBy(a => a.Quiniela.Nombre)
                .Select(a => new MiQuinielaDto { Id = a.QuinielaId, Nombre = a.Quiniela.Nombre })
                .ToListAsync(ct);

            return Result.Success(rows);
        }
    }
}

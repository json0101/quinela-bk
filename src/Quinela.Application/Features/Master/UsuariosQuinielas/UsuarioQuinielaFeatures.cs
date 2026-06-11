using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quinela.Application.Common.Abstractions;
using Quinela.Application.Common.Results;
using Quinela.Domain.Entities;
using UserApp.Service.Services.Users;

namespace Quinela.Application.Features.Master.UsuariosQuinielas
{
    public static class UsuarioQuinielaErrors
    {
        public static readonly Error NotFound = Error.NotFound("UsuarioQuiniela.NotFound", "No se encontró el acceso.");
        public static readonly Error Duplicate = Error.Conflict("UsuarioQuiniela.Duplicate", "El usuario ya tiene acceso a esa quiniela.");
        public static readonly Error QuinielaNotFound = Error.NotFound("UsuarioQuiniela.QuinielaNotFound", "No se encontró la quiniela.");
        public static readonly Error UsuarioNotFound = Error.NotFound("UsuarioQuiniela.UsuarioNotFound", "No se encontró el usuario.");
    }

    // Usuario de UserApp resuelto desde el servicio del paquete (sin acoplar a Postgres).
    public class UsuarioAppRow
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    /// <summary>
    /// Lista los usuarios de UserApp usando IUserService.GetGrid() del paquete (id + username + email),
    /// para no acoplarse al esquema de Postgres. Los usuarios viven en otra base de datos.
    /// </summary>
    public sealed class UsuariosAppReader
    {
        private readonly IUserService _users;
        public UsuariosAppReader(IUserService users) => _users = users;

        public Task<List<UsuarioAppRow>> ListarAsync(CancellationToken ct)
        {
            var rows = _users.GetGrid()
                .Where(u => u.Active)
                .Select(u => new UsuarioAppRow { UserId = u.Id, UserName = u.UserName, Email = u.Email })
                .OrderBy(u => u.UserName)
                .ToList();
            return Task.FromResult(rows);
        }
    }

    // ----- Listar usuarios de UserApp (combo del formulario) -----
    public sealed record GetUsuariosAppQuery() : IRequest<Result<List<UsuarioAppDto>>>;

    internal sealed class GetUsuariosAppHandler : IRequestHandler<GetUsuariosAppQuery, Result<List<UsuarioAppDto>>>
    {
        private readonly UsuariosAppReader _reader;
        public GetUsuariosAppHandler(UsuariosAppReader reader) => _reader = reader;

        public async Task<Result<List<UsuarioAppDto>>> Handle(GetUsuariosAppQuery request, CancellationToken ct)
        {
            var rows = await _reader.ListarAsync(ct);
            return Result.Success(rows.Select(u => new UsuarioAppDto
            {
                UserId = u.UserId, UserName = u.UserName, Email = u.Email
            }).ToList());
        }
    }

    // ----- GetAll accesos -----
    public sealed record GetAllUsuariosQuinielasQuery() : IRequest<Result<List<UsuarioQuinielaDto>>>;

    internal sealed class GetAllUsuariosQuinielasHandler : IRequestHandler<GetAllUsuariosQuinielasQuery, Result<List<UsuarioQuinielaDto>>>
    {
        private readonly IRepository<UsuarioQuiniela> _repo;
        private readonly UsuariosAppReader _reader;
        public GetAllUsuariosQuinielasHandler(IRepository<UsuarioQuiniela> repo, UsuariosAppReader reader)
        { _repo = repo; _reader = reader; }

        public async Task<Result<List<UsuarioQuinielaDto>>> Handle(GetAllUsuariosQuinielasQuery request, CancellationToken ct)
        {
            var rows = await _repo.GetDbSet().AsNoTracking().Include(x => x.Quiniela)
                .OrderBy(x => x.UserId).ThenBy(x => x.QuinielaId)
                .ToListAsync(ct);

            // Resolver nombre/correo del usuario desde UserApp (están en otra base de datos).
            var usuarios = (await _reader.ListarAsync(ct)).ToDictionary(u => u.UserId);

            return Result.Success(rows.Select(r => new UsuarioQuinielaDto
            {
                Id = r.Id,
                UserId = r.UserId,
                UserName = usuarios.TryGetValue(r.UserId, out var u) ? u.UserName : $"(usuario {r.UserId})",
                Email = usuarios.TryGetValue(r.UserId, out var u2) ? u2.Email : string.Empty,
                QuinielaId = r.QuinielaId,
                Quiniela = r.Quiniela != null ? r.Quiniela.Nombre : string.Empty,
                Active = r.Active,
                CreatedAt = r.CreatedAt, CreatedBy = r.CreatedBy, UpdatedAt = r.UpdatedAt, UpdatedBy = r.UpdatedBy
            }).ToList());
        }
    }

    // ----- Create -----
    public sealed record CreateUsuarioQuinielaCommand(int UserId, int QuinielaId, bool Active) : IRequest<Result<UsuarioQuinielaDto>>;

    public sealed class CreateUsuarioQuinielaValidator : AbstractValidator<CreateUsuarioQuinielaCommand>
    {
        public CreateUsuarioQuinielaValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("El usuario es requerido.");
            RuleFor(x => x.QuinielaId).GreaterThan(0).WithMessage("La quiniela es requerida.");
        }
    }

    internal sealed class CreateUsuarioQuinielaHandler : IRequestHandler<CreateUsuarioQuinielaCommand, Result<UsuarioQuinielaDto>>
    {
        private readonly IRepository<UsuarioQuiniela> _repo;
        private readonly IRepository<Quiniela> _quinielas;
        private readonly UsuariosAppReader _reader;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUser _currentUser;
        public CreateUsuarioQuinielaHandler(IRepository<UsuarioQuiniela> repo, IRepository<Quiniela> quinielas,
            UsuariosAppReader reader, IUnitOfWork uow, ICurrentUser currentUser)
        { _repo = repo; _quinielas = quinielas; _reader = reader; _uow = uow; _currentUser = currentUser; }

        public async Task<Result<UsuarioQuinielaDto>> Handle(CreateUsuarioQuinielaCommand cmd, CancellationToken ct)
        {
            var usuarios = await _reader.ListarAsync(ct);
            var usuario = usuarios.FirstOrDefault(u => u.UserId == cmd.UserId);
            if (usuario is null) return Result.Failure<UsuarioQuinielaDto>(UsuarioQuinielaErrors.UsuarioNotFound);

            var quiniela = await _quinielas.GetDbSet().AsNoTracking().FirstOrDefaultAsync(q => q.Id == cmd.QuinielaId, ct);
            if (quiniela is null) return Result.Failure<UsuarioQuinielaDto>(UsuarioQuinielaErrors.QuinielaNotFound);

            var dup = await _repo.GetDbSet().AsNoTracking()
                .AnyAsync(x => x.UserId == cmd.UserId && x.QuinielaId == cmd.QuinielaId, ct);
            if (dup) return Result.Failure<UsuarioQuinielaDto>(UsuarioQuinielaErrors.Duplicate);

            var entity = new UsuarioQuiniela
            {
                UserId = cmd.UserId, QuinielaId = cmd.QuinielaId, Active = cmd.Active,
                CreatedAt = DateTime.UtcNow, CreatedBy = _currentUser.UserName
            };
            _repo.Insert(entity);
            await _uow.SaveChangesAsync(ct);

            return Result.Success(new UsuarioQuinielaDto
            {
                Id = entity.Id, UserId = entity.UserId, UserName = usuario.UserName, Email = usuario.Email,
                QuinielaId = entity.QuinielaId, Quiniela = quiniela.Nombre, Active = entity.Active,
                CreatedAt = entity.CreatedAt, CreatedBy = entity.CreatedBy
            });
        }
    }

    // ----- Update -----
    public sealed record UpdateUsuarioQuinielaCommand(int Id, int UserId, int QuinielaId, bool Active) : IRequest<Result<UsuarioQuinielaDto>>;

    public sealed class UpdateUsuarioQuinielaValidator : AbstractValidator<UpdateUsuarioQuinielaCommand>
    {
        public UpdateUsuarioQuinielaValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("El usuario es requerido.");
            RuleFor(x => x.QuinielaId).GreaterThan(0).WithMessage("La quiniela es requerida.");
        }
    }

    internal sealed class UpdateUsuarioQuinielaHandler : IRequestHandler<UpdateUsuarioQuinielaCommand, Result<UsuarioQuinielaDto>>
    {
        private readonly IRepository<UsuarioQuiniela> _repo;
        private readonly IRepository<Quiniela> _quinielas;
        private readonly UsuariosAppReader _reader;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUser _currentUser;
        public UpdateUsuarioQuinielaHandler(IRepository<UsuarioQuiniela> repo, IRepository<Quiniela> quinielas,
            UsuariosAppReader reader, IUnitOfWork uow, ICurrentUser currentUser)
        { _repo = repo; _quinielas = quinielas; _reader = reader; _uow = uow; _currentUser = currentUser; }

        public async Task<Result<UsuarioQuinielaDto>> Handle(UpdateUsuarioQuinielaCommand cmd, CancellationToken ct)
        {
            var entity = await _repo.GetDbSet().FirstOrDefaultAsync(x => x.Id == cmd.Id, ct);
            if (entity is null) return Result.Failure<UsuarioQuinielaDto>(UsuarioQuinielaErrors.NotFound);

            var usuarios = await _reader.ListarAsync(ct);
            var usuario = usuarios.FirstOrDefault(u => u.UserId == cmd.UserId);
            if (usuario is null) return Result.Failure<UsuarioQuinielaDto>(UsuarioQuinielaErrors.UsuarioNotFound);

            var quiniela = await _quinielas.GetDbSet().AsNoTracking().FirstOrDefaultAsync(q => q.Id == cmd.QuinielaId, ct);
            if (quiniela is null) return Result.Failure<UsuarioQuinielaDto>(UsuarioQuinielaErrors.QuinielaNotFound);

            var dup = await _repo.GetDbSet().AsNoTracking()
                .AnyAsync(x => x.Id != cmd.Id && x.UserId == cmd.UserId && x.QuinielaId == cmd.QuinielaId, ct);
            if (dup) return Result.Failure<UsuarioQuinielaDto>(UsuarioQuinielaErrors.Duplicate);

            entity.UserId = cmd.UserId;
            entity.QuinielaId = cmd.QuinielaId;
            entity.Active = cmd.Active;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = _currentUser.UserName;
            await _uow.SaveChangesAsync(ct);

            return Result.Success(new UsuarioQuinielaDto
            {
                Id = entity.Id, UserId = entity.UserId, UserName = usuario.UserName, Email = usuario.Email,
                QuinielaId = entity.QuinielaId, Quiniela = quiniela.Nombre, Active = entity.Active,
                CreatedAt = entity.CreatedAt, CreatedBy = entity.CreatedBy,
                UpdatedAt = entity.UpdatedAt, UpdatedBy = entity.UpdatedBy
            });
        }
    }

    // ----- Delete (físico: es una tabla de enlace) -----
    public sealed record DeleteUsuarioQuinielaCommand(int Id) : IRequest<Result>;

    internal sealed class DeleteUsuarioQuinielaHandler : IRequestHandler<DeleteUsuarioQuinielaCommand, Result>
    {
        private readonly IRepository<UsuarioQuiniela> _repo;
        private readonly IUnitOfWork _uow;
        public DeleteUsuarioQuinielaHandler(IRepository<UsuarioQuiniela> repo, IUnitOfWork uow)
        { _repo = repo; _uow = uow; }

        public async Task<Result> Handle(DeleteUsuarioQuinielaCommand cmd, CancellationToken ct)
        {
            var entity = await _repo.GetDbSet().FirstOrDefaultAsync(x => x.Id == cmd.Id, ct);
            if (entity is null) return Result.Failure(UsuarioQuinielaErrors.NotFound);

            _repo.Remove(entity);
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}

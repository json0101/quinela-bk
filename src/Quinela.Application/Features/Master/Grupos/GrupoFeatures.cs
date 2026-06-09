using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quinela.Application.Common.Abstractions;
using Quinela.Application.Common.Results;
using Quinela.Domain.Entities;

namespace Quinela.Application.Features.Master.Grupos
{
    public static class GrupoErrors
    {
        public static readonly Error NotFound = Error.NotFound("Grupo.NotFound", "No se encontró el grupo.");
        public static readonly Error Duplicate = Error.Conflict("Grupo.Duplicate", "Ya existe un grupo con ese nombre.");
    }

    // ----- GetAll -----
    public sealed record GetAllGruposQuery() : IRequest<Result<List<GrupoDto>>>;

    internal sealed class GetAllGruposHandler : IRequestHandler<GetAllGruposQuery, Result<List<GrupoDto>>>
    {
        private readonly IRepository<Grupo> _repo;
        public GetAllGruposHandler(IRepository<Grupo> repo) => _repo = repo;

        public async Task<Result<List<GrupoDto>>> Handle(GetAllGruposQuery request, CancellationToken ct)
        {
            var rows = await _repo.GetDbSet().AsNoTracking().OrderBy(x => x.Nombre)
                .Select(x => new GrupoDto
                {
                    Id = x.Id, Nombre = x.Nombre, Active = x.Active,
                    CreatedAt = x.CreatedAt, CreatedBy = x.CreatedBy, UpdatedAt = x.UpdatedAt, UpdatedBy = x.UpdatedBy
                }).ToListAsync(ct);
            return Result.Success(rows);
        }
    }

    // ----- GetById -----
    public sealed record GetGrupoByIdQuery(int Id) : IRequest<Result<GrupoDto>>;

    internal sealed class GetGrupoByIdHandler : IRequestHandler<GetGrupoByIdQuery, Result<GrupoDto>>
    {
        private readonly IRepository<Grupo> _repo;
        public GetGrupoByIdHandler(IRepository<Grupo> repo) => _repo = repo;

        public async Task<Result<GrupoDto>> Handle(GetGrupoByIdQuery request, CancellationToken ct)
        {
            var x = await _repo.GetDbSet().AsNoTracking().FirstOrDefaultAsync(g => g.Id == request.Id, ct);
            if (x is null) return Result.Failure<GrupoDto>(GrupoErrors.NotFound);
            return Result.Success(new GrupoDto
            {
                Id = x.Id, Nombre = x.Nombre, Active = x.Active,
                CreatedAt = x.CreatedAt, CreatedBy = x.CreatedBy, UpdatedAt = x.UpdatedAt, UpdatedBy = x.UpdatedBy
            });
        }
    }

    // ----- Create -----
    public sealed record CreateGrupoCommand(string Nombre, bool Active) : IRequest<Result<GrupoDto>>;

    public sealed class CreateGrupoValidator : AbstractValidator<CreateGrupoCommand>
    {
        public CreateGrupoValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre es requerido.");
            RuleFor(x => x.Nombre).MaximumLength(5).WithMessage("El nombre no debe superar 5 caracteres.");
        }
    }

    internal sealed class CreateGrupoHandler : IRequestHandler<CreateGrupoCommand, Result<GrupoDto>>
    {
        private readonly IRepository<Grupo> _repo;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUser _currentUser;
        public CreateGrupoHandler(IRepository<Grupo> repo, IUnitOfWork uow, ICurrentUser currentUser)
        { _repo = repo; _uow = uow; _currentUser = currentUser; }

        public async Task<Result<GrupoDto>> Handle(CreateGrupoCommand cmd, CancellationToken ct)
        {
            var nombre = cmd.Nombre.Trim();
            var exists = await _repo.GetDbSet().AsNoTracking()
                .AnyAsync(x => x.Nombre.ToLower() == nombre.ToLower(), ct);
            if (exists) return Result.Failure<GrupoDto>(GrupoErrors.Duplicate);

            var entity = new Grupo
            {
                Nombre = nombre, Active = cmd.Active,
                CreatedAt = DateTime.UtcNow, CreatedBy = _currentUser.UserName
            };
            _repo.Insert(entity);
            await _uow.SaveChangesAsync(ct);

            return Result.Success(new GrupoDto
            {
                Id = entity.Id, Nombre = entity.Nombre, Active = entity.Active,
                CreatedAt = entity.CreatedAt, CreatedBy = entity.CreatedBy
            });
        }
    }

    // ----- Update -----
    public sealed record UpdateGrupoCommand(int Id, string Nombre, bool Active) : IRequest<Result<GrupoDto>>;

    public sealed class UpdateGrupoValidator : AbstractValidator<UpdateGrupoCommand>
    {
        public UpdateGrupoValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre es requerido.");
            RuleFor(x => x.Nombre).MaximumLength(5).WithMessage("El nombre no debe superar 5 caracteres.");
        }
    }

    internal sealed class UpdateGrupoHandler : IRequestHandler<UpdateGrupoCommand, Result<GrupoDto>>
    {
        private readonly IRepository<Grupo> _repo;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUser _currentUser;
        public UpdateGrupoHandler(IRepository<Grupo> repo, IUnitOfWork uow, ICurrentUser currentUser)
        { _repo = repo; _uow = uow; _currentUser = currentUser; }

        public async Task<Result<GrupoDto>> Handle(UpdateGrupoCommand cmd, CancellationToken ct)
        {
            var entity = await _repo.GetDbSet().FirstOrDefaultAsync(x => x.Id == cmd.Id, ct);
            if (entity is null) return Result.Failure<GrupoDto>(GrupoErrors.NotFound);

            var nombre = cmd.Nombre.Trim();
            var dup = await _repo.GetDbSet().AsNoTracking()
                .AnyAsync(x => x.Id != cmd.Id && x.Nombre.ToLower() == nombre.ToLower(), ct);
            if (dup) return Result.Failure<GrupoDto>(GrupoErrors.Duplicate);

            entity.Nombre = nombre;
            entity.Active = cmd.Active;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = _currentUser.UserName;
            await _uow.SaveChangesAsync(ct);

            return Result.Success(new GrupoDto
            {
                Id = entity.Id, Nombre = entity.Nombre, Active = entity.Active,
                CreatedAt = entity.CreatedAt, CreatedBy = entity.CreatedBy,
                UpdatedAt = entity.UpdatedAt, UpdatedBy = entity.UpdatedBy
            });
        }
    }

    // ----- Delete -----
    public sealed record DeleteGrupoCommand(int Id) : IRequest<Result>;

    internal sealed class DeleteGrupoHandler : IRequestHandler<DeleteGrupoCommand, Result>
    {
        private readonly IRepository<Grupo> _repo;
        private readonly IUnitOfWork _uow;
        public DeleteGrupoHandler(IRepository<Grupo> repo, IUnitOfWork uow)
        { _repo = repo; _uow = uow; }

        public async Task<Result> Handle(DeleteGrupoCommand cmd, CancellationToken ct)
        {
            var entity = await _repo.GetDbSet().FirstOrDefaultAsync(x => x.Id == cmd.Id, ct);
            if (entity is null) return Result.Failure(GrupoErrors.NotFound);

            _repo.Remove(entity);
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}

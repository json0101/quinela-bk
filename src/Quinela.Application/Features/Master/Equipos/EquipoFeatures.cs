using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quinela.Application.Common.Abstractions;
using Quinela.Application.Common.Results;
using Quinela.Domain.Entities;

namespace Quinela.Application.Features.Master.Equipos
{
    public static class EquipoErrors
    {
        public static readonly Error NotFound = Error.NotFound("Equipo.NotFound", "No se encontró el equipo.");
        public static readonly Error Duplicate = Error.Conflict("Equipo.Duplicate", "Ya existe un equipo con ese nombre en el torneo.");
        public static readonly Error TorneoNotFound = Error.NotFound("Equipo.TorneoNotFound", "No se encontró el torneo.");
    }

    // ----- GetAll -----
    public sealed record GetAllEquiposQuery() : IRequest<Result<List<EquipoDto>>>;

    internal sealed class GetAllEquiposHandler : IRequestHandler<GetAllEquiposQuery, Result<List<EquipoDto>>>
    {
        private readonly IRepository<Equipo> _repo;
        public GetAllEquiposHandler(IRepository<Equipo> repo) => _repo = repo;

        public async Task<Result<List<EquipoDto>>> Handle(GetAllEquiposQuery request, CancellationToken ct)
        {
            var rows = await _repo.GetDbSet().AsNoTracking().OrderBy(x => x.Nombre)
                .Select(x => new EquipoDto
                {
                    Id = x.Id, Nombre = x.Nombre, Confederacion = x.Confederacion, Anfitrion = x.Anfitrion, UrlBandera = x.UrlBandera,
                    TorneoId = x.TorneoId, Torneo = x.Torneo.Descripcion, Active = x.Active,
                    CreatedAt = x.CreatedAt, CreatedBy = x.CreatedBy, UpdatedAt = x.UpdatedAt, UpdatedBy = x.UpdatedBy
                }).ToListAsync(ct);
            return Result.Success(rows);
        }
    }

    // ----- GetById -----
    public sealed record GetEquipoByIdQuery(int Id) : IRequest<Result<EquipoDto>>;

    internal sealed class GetEquipoByIdHandler : IRequestHandler<GetEquipoByIdQuery, Result<EquipoDto>>
    {
        private readonly IRepository<Equipo> _repo;
        public GetEquipoByIdHandler(IRepository<Equipo> repo) => _repo = repo;

        public async Task<Result<EquipoDto>> Handle(GetEquipoByIdQuery request, CancellationToken ct)
        {
            var x = await _repo.GetDbSet().AsNoTracking()
                .Select(e => new EquipoDto
                {
                    Id = e.Id, Nombre = e.Nombre, Confederacion = e.Confederacion, Anfitrion = e.Anfitrion, UrlBandera = e.UrlBandera,
                    TorneoId = e.TorneoId, Torneo = e.Torneo.Descripcion, Active = e.Active,
                    CreatedAt = e.CreatedAt, CreatedBy = e.CreatedBy, UpdatedAt = e.UpdatedAt, UpdatedBy = e.UpdatedBy
                })
                .FirstOrDefaultAsync(e => e.Id == request.Id, ct);
            if (x is null) return Result.Failure<EquipoDto>(EquipoErrors.NotFound);
            return Result.Success(x);
        }
    }

    // ----- Create -----
    public sealed record CreateEquipoCommand(string Nombre, string Confederacion, bool Anfitrion, string? UrlBandera, int TorneoId, bool Active) : IRequest<Result<EquipoDto>>;

    public sealed class CreateEquipoValidator : AbstractValidator<CreateEquipoCommand>
    {
        public CreateEquipoValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre es requerido.").MaximumLength(120);
            RuleFor(x => x.Confederacion).NotEmpty().WithMessage("La confederación es requerida.").MaximumLength(20);
            RuleFor(x => x.TorneoId).GreaterThan(0).WithMessage("El torneo es requerido.");
        }
    }

    internal sealed class CreateEquipoHandler : IRequestHandler<CreateEquipoCommand, Result<EquipoDto>>
    {
        private readonly IRepository<Equipo> _repo;
        private readonly IRepository<Torneo> _torneos;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUser _currentUser;
        public CreateEquipoHandler(IRepository<Equipo> repo, IRepository<Torneo> torneos, IUnitOfWork uow, ICurrentUser currentUser)
        { _repo = repo; _torneos = torneos; _uow = uow; _currentUser = currentUser; }

        public async Task<Result<EquipoDto>> Handle(CreateEquipoCommand cmd, CancellationToken ct)
        {
            if (!await _torneos.GetDbSet().AsNoTracking().AnyAsync(t => t.Id == cmd.TorneoId, ct))
                return Result.Failure<EquipoDto>(EquipoErrors.TorneoNotFound);

            var nombre = cmd.Nombre.Trim();
            var exists = await _repo.GetDbSet().AsNoTracking()
                .AnyAsync(x => x.TorneoId == cmd.TorneoId && x.Nombre.ToLower() == nombre.ToLower(), ct);
            if (exists) return Result.Failure<EquipoDto>(EquipoErrors.Duplicate);

            var entity = new Equipo
            {
                Nombre = nombre, Confederacion = cmd.Confederacion.Trim(), Anfitrion = cmd.Anfitrion,
                UrlBandera = cmd.UrlBandera, TorneoId = cmd.TorneoId, Active = cmd.Active,
                CreatedAt = DateTime.UtcNow, CreatedBy = _currentUser.UserName
            };
            _repo.Insert(entity);
            await _uow.SaveChangesAsync(ct);

            return Result.Success(new EquipoDto
            {
                Id = entity.Id, Nombre = entity.Nombre, Confederacion = entity.Confederacion,
                Anfitrion = entity.Anfitrion, UrlBandera = entity.UrlBandera, TorneoId = entity.TorneoId, Active = entity.Active,
                CreatedAt = entity.CreatedAt, CreatedBy = entity.CreatedBy
            });
        }
    }

    // ----- Update -----
    public sealed record UpdateEquipoCommand(int Id, string Nombre, string Confederacion, bool Anfitrion, string? UrlBandera, int TorneoId, bool Active) : IRequest<Result<EquipoDto>>;

    public sealed class UpdateEquipoValidator : AbstractValidator<UpdateEquipoCommand>
    {
        public UpdateEquipoValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre es requerido.").MaximumLength(120);
            RuleFor(x => x.Confederacion).NotEmpty().WithMessage("La confederación es requerida.").MaximumLength(20);
            RuleFor(x => x.TorneoId).GreaterThan(0).WithMessage("El torneo es requerido.");
        }
    }

    internal sealed class UpdateEquipoHandler : IRequestHandler<UpdateEquipoCommand, Result<EquipoDto>>
    {
        private readonly IRepository<Equipo> _repo;
        private readonly IRepository<Torneo> _torneos;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUser _currentUser;
        public UpdateEquipoHandler(IRepository<Equipo> repo, IRepository<Torneo> torneos, IUnitOfWork uow, ICurrentUser currentUser)
        { _repo = repo; _torneos = torneos; _uow = uow; _currentUser = currentUser; }

        public async Task<Result<EquipoDto>> Handle(UpdateEquipoCommand cmd, CancellationToken ct)
        {
            var entity = await _repo.GetDbSet().FirstOrDefaultAsync(x => x.Id == cmd.Id, ct);
            if (entity is null) return Result.Failure<EquipoDto>(EquipoErrors.NotFound);

            if (!await _torneos.GetDbSet().AsNoTracking().AnyAsync(t => t.Id == cmd.TorneoId, ct))
                return Result.Failure<EquipoDto>(EquipoErrors.TorneoNotFound);

            var nombre = cmd.Nombre.Trim();
            var dup = await _repo.GetDbSet().AsNoTracking()
                .AnyAsync(x => x.Id != cmd.Id && x.TorneoId == cmd.TorneoId && x.Nombre.ToLower() == nombre.ToLower(), ct);
            if (dup) return Result.Failure<EquipoDto>(EquipoErrors.Duplicate);

            entity.Nombre = nombre;
            entity.Confederacion = cmd.Confederacion.Trim();
            entity.Anfitrion = cmd.Anfitrion;
            entity.UrlBandera = cmd.UrlBandera;
            entity.TorneoId = cmd.TorneoId;
            entity.Active = cmd.Active;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = _currentUser.UserName;
            await _uow.SaveChangesAsync(ct);

            return Result.Success(new EquipoDto
            {
                Id = entity.Id, Nombre = entity.Nombre, Confederacion = entity.Confederacion,
                Anfitrion = entity.Anfitrion, UrlBandera = entity.UrlBandera, TorneoId = entity.TorneoId, Active = entity.Active,
                CreatedAt = entity.CreatedAt, CreatedBy = entity.CreatedBy,
                UpdatedAt = entity.UpdatedAt, UpdatedBy = entity.UpdatedBy
            });
        }
    }

    // ----- Delete -----
    public sealed record DeleteEquipoCommand(int Id) : IRequest<Result>;

    internal sealed class DeleteEquipoHandler : IRequestHandler<DeleteEquipoCommand, Result>
    {
        private readonly IRepository<Equipo> _repo;
        private readonly IUnitOfWork _uow;
        public DeleteEquipoHandler(IRepository<Equipo> repo, IUnitOfWork uow)
        { _repo = repo; _uow = uow; }

        public async Task<Result> Handle(DeleteEquipoCommand cmd, CancellationToken ct)
        {
            var entity = await _repo.GetDbSet().FirstOrDefaultAsync(x => x.Id == cmd.Id, ct);
            if (entity is null) return Result.Failure(EquipoErrors.NotFound);

            _repo.Remove(entity);
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}

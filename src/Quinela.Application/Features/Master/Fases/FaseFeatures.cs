using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quinela.Application.Common.Abstractions;
using Quinela.Application.Common.Results;
using Quinela.Domain.Entities;

namespace Quinela.Application.Features.Master.Fases
{
    public class FaseDto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int TorneoId { get; set; }
        public string Torneo { get; set; } = string.Empty;
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class FaseCreateDto { public string Descripcion { get; set; } = string.Empty; public int TorneoId { get; set; } public bool Active { get; set; } = true; }
    public class FaseUpdateDto { public string Descripcion { get; set; } = string.Empty; public int TorneoId { get; set; } public bool Active { get; set; } }

    public static class FaseErrors
    {
        public static readonly Error NotFound = Error.NotFound("Fase.NotFound", "No se encontró la fase.");
        public static readonly Error TorneoNotFound = Error.NotFound("Fase.TorneoNotFound", "No se encontró el torneo indicado.");
        public static readonly Error Duplicate = Error.Conflict("Fase.Duplicate", "Ya existe una fase con esa descripción para el torneo.");
    }

    // ----- GetAll -----
    public sealed record GetAllFasesQuery() : IRequest<Result<List<FaseDto>>>;

    internal sealed class GetAllFasesHandler : IRequestHandler<GetAllFasesQuery, Result<List<FaseDto>>>
    {
        private readonly IRepository<Fase> _repo;
        public GetAllFasesHandler(IRepository<Fase> repo) => _repo = repo;

        public async Task<Result<List<FaseDto>>> Handle(GetAllFasesQuery request, CancellationToken ct)
        {
            var rows = await _repo.GetDbSet().AsNoTracking().OrderBy(x => x.Descripcion)
                .Select(x => new FaseDto
                {
                    Id = x.Id, Descripcion = x.Descripcion,
                    TorneoId = x.TorneoId, Torneo = x.Torneo.Descripcion, Active = x.Active,
                    CreatedAt = x.CreatedAt, CreatedBy = x.CreatedBy, UpdatedAt = x.UpdatedAt, UpdatedBy = x.UpdatedBy
                }).ToListAsync(ct);
            return Result.Success(rows);
        }
    }

    // ----- GetById -----
    public sealed record GetFaseByIdQuery(int Id) : IRequest<Result<FaseDto>>;

    internal sealed class GetFaseByIdHandler : IRequestHandler<GetFaseByIdQuery, Result<FaseDto>>
    {
        private readonly IRepository<Fase> _repo;
        public GetFaseByIdHandler(IRepository<Fase> repo) => _repo = repo;

        public async Task<Result<FaseDto>> Handle(GetFaseByIdQuery request, CancellationToken ct)
        {
            var x = await _repo.GetDbSet().AsNoTracking()
                .Select(f => new FaseDto
                {
                    Id = f.Id, Descripcion = f.Descripcion,
                    TorneoId = f.TorneoId, Torneo = f.Torneo.Descripcion, Active = f.Active,
                    CreatedAt = f.CreatedAt, CreatedBy = f.CreatedBy, UpdatedAt = f.UpdatedAt, UpdatedBy = f.UpdatedBy
                })
                .FirstOrDefaultAsync(f => f.Id == request.Id, ct);
            if (x is null) return Result.Failure<FaseDto>(FaseErrors.NotFound);
            return Result.Success(x);
        }
    }

    // ----- Create -----
    public sealed record CreateFaseCommand(string Descripcion, int TorneoId, bool Active) : IRequest<Result<FaseDto>>;

    public sealed class CreateFaseValidator : AbstractValidator<CreateFaseCommand>
    {
        public CreateFaseValidator()
        {
            RuleFor(x => x.Descripcion).NotEmpty().WithMessage("La descripción es requerida.").MaximumLength(120);
            RuleFor(x => x.TorneoId).GreaterThan(0).WithMessage("El torneo es requerido.");
        }
    }

    internal sealed class CreateFaseHandler : IRequestHandler<CreateFaseCommand, Result<FaseDto>>
    {
        private readonly IRepository<Fase> _repo;
        private readonly IRepository<Torneo> _torneos;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUser _currentUser;
        public CreateFaseHandler(IRepository<Fase> repo, IRepository<Torneo> torneos, IUnitOfWork uow, ICurrentUser currentUser)
        { _repo = repo; _torneos = torneos; _uow = uow; _currentUser = currentUser; }

        public async Task<Result<FaseDto>> Handle(CreateFaseCommand cmd, CancellationToken ct)
        {
            if (!await _torneos.GetDbSet().AsNoTracking().AnyAsync(t => t.Id == cmd.TorneoId, ct))
                return Result.Failure<FaseDto>(FaseErrors.TorneoNotFound);

            var descripcion = cmd.Descripcion.Trim();
            var exists = await _repo.GetDbSet().AsNoTracking()
                .AnyAsync(x => x.TorneoId == cmd.TorneoId && x.Descripcion.ToLower() == descripcion.ToLower(), ct);
            if (exists) return Result.Failure<FaseDto>(FaseErrors.Duplicate);

            var entity = new Fase
            {
                Descripcion = descripcion, TorneoId = cmd.TorneoId, Active = cmd.Active,
                CreatedAt = DateTime.UtcNow, CreatedBy = _currentUser.UserName
            };
            _repo.Insert(entity);
            await _uow.SaveChangesAsync(ct);

            return Result.Success(new FaseDto
            {
                Id = entity.Id, Descripcion = entity.Descripcion,
                TorneoId = entity.TorneoId, Active = entity.Active,
                CreatedAt = entity.CreatedAt, CreatedBy = entity.CreatedBy
            });
        }
    }

    // ----- Update -----
    public sealed record UpdateFaseCommand(int Id, string Descripcion, int TorneoId, bool Active) : IRequest<Result<FaseDto>>;

    public sealed class UpdateFaseValidator : AbstractValidator<UpdateFaseCommand>
    {
        public UpdateFaseValidator()
        {
            RuleFor(x => x.Descripcion).NotEmpty().WithMessage("La descripción es requerida.").MaximumLength(120);
            RuleFor(x => x.TorneoId).GreaterThan(0).WithMessage("El torneo es requerido.");
        }
    }

    internal sealed class UpdateFaseHandler : IRequestHandler<UpdateFaseCommand, Result<FaseDto>>
    {
        private readonly IRepository<Fase> _repo;
        private readonly IRepository<Torneo> _torneos;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUser _currentUser;
        public UpdateFaseHandler(IRepository<Fase> repo, IRepository<Torneo> torneos, IUnitOfWork uow, ICurrentUser currentUser)
        { _repo = repo; _torneos = torneos; _uow = uow; _currentUser = currentUser; }

        public async Task<Result<FaseDto>> Handle(UpdateFaseCommand cmd, CancellationToken ct)
        {
            var entity = await _repo.GetDbSet().FirstOrDefaultAsync(x => x.Id == cmd.Id, ct);
            if (entity is null) return Result.Failure<FaseDto>(FaseErrors.NotFound);

            if (!await _torneos.GetDbSet().AsNoTracking().AnyAsync(t => t.Id == cmd.TorneoId, ct))
                return Result.Failure<FaseDto>(FaseErrors.TorneoNotFound);

            var descripcion = cmd.Descripcion.Trim();
            var dup = await _repo.GetDbSet().AsNoTracking()
                .AnyAsync(x => x.Id != cmd.Id && x.TorneoId == cmd.TorneoId && x.Descripcion.ToLower() == descripcion.ToLower(), ct);
            if (dup) return Result.Failure<FaseDto>(FaseErrors.Duplicate);

            entity.Descripcion = descripcion;
            entity.TorneoId = cmd.TorneoId;
            entity.Active = cmd.Active;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = _currentUser.UserName;
            await _uow.SaveChangesAsync(ct);

            return Result.Success(new FaseDto
            {
                Id = entity.Id, Descripcion = entity.Descripcion,
                TorneoId = entity.TorneoId, Active = entity.Active,
                CreatedAt = entity.CreatedAt, CreatedBy = entity.CreatedBy,
                UpdatedAt = entity.UpdatedAt, UpdatedBy = entity.UpdatedBy
            });
        }
    }

    // ----- Delete -----
    public sealed record DeleteFaseCommand(int Id) : IRequest<Result>;

    internal sealed class DeleteFaseHandler : IRequestHandler<DeleteFaseCommand, Result>
    {
        private readonly IRepository<Fase> _repo;
        private readonly IUnitOfWork _uow;
        public DeleteFaseHandler(IRepository<Fase> repo, IUnitOfWork uow)
        { _repo = repo; _uow = uow; }

        public async Task<Result> Handle(DeleteFaseCommand cmd, CancellationToken ct)
        {
            var entity = await _repo.GetDbSet().FirstOrDefaultAsync(x => x.Id == cmd.Id, ct);
            if (entity is null) return Result.Failure(FaseErrors.NotFound);

            _repo.Remove(entity);
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}

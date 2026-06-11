using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quinela.Application.Common.Abstractions;
using Quinela.Application.Common.Results;
using QuinielaEntity = Quinela.Domain.Entities.Quiniela;
using Quinela.Domain.Entities;

namespace Quinela.Application.Features.Master.Quinielas
{
    public static class QuinielaErrors
    {
        public static readonly Error NotFound = Error.NotFound("Quiniela.NotFound", "No se encontró la quiniela.");
        public static readonly Error TorneoNotFound = Error.NotFound("Quiniela.TorneoNotFound", "No se encontró el torneo.");
    }

    internal static class QuinielaMapper
    {
        public static QuinielaDto ToDto(QuinielaEntity x) => new()
        {
            Id = x.Id,
            Nombre = x.Nombre,
            Reglas = x.Reglas,
            TorneoId = x.TorneoId,
            Torneo = x.Torneo != null ? x.Torneo.Descripcion : string.Empty,
            Active = x.Active,
            CreatedAt = x.CreatedAt,
            CreatedBy = x.CreatedBy,
            UpdatedAt = x.UpdatedAt,
            UpdatedBy = x.UpdatedBy
        };
    }

    // ----- GetAll (activas) -----
    public sealed record GetAllQuinielasQuery() : IRequest<Result<List<QuinielaDto>>>;

    internal sealed class GetAllQuinielasHandler : IRequestHandler<GetAllQuinielasQuery, Result<List<QuinielaDto>>>
    {
        private readonly IRepository<QuinielaEntity> _repo;
        public GetAllQuinielasHandler(IRepository<QuinielaEntity> repo) => _repo = repo;

        public async Task<Result<List<QuinielaDto>>> Handle(GetAllQuinielasQuery request, CancellationToken ct)
        {
            var rows = await _repo.GetDbSet().AsNoTracking().Include(x => x.Torneo)
                .Where(x => x.Active)
                .OrderBy(x => x.Nombre)
                .ToListAsync(ct);
            return Result.Success(rows.Select(QuinielaMapper.ToDto).ToList());
        }
    }

    // ----- GetById -----
    public sealed record GetQuinielaByIdQuery(int Id) : IRequest<Result<QuinielaDto>>;

    internal sealed class GetQuinielaByIdHandler : IRequestHandler<GetQuinielaByIdQuery, Result<QuinielaDto>>
    {
        private readonly IRepository<QuinielaEntity> _repo;
        public GetQuinielaByIdHandler(IRepository<QuinielaEntity> repo) => _repo = repo;

        public async Task<Result<QuinielaDto>> Handle(GetQuinielaByIdQuery request, CancellationToken ct)
        {
            var x = await _repo.GetDbSet().AsNoTracking().Include(q => q.Torneo).FirstOrDefaultAsync(q => q.Id == request.Id, ct);
            if (x is null) return Result.Failure<QuinielaDto>(QuinielaErrors.NotFound);
            return Result.Success(QuinielaMapper.ToDto(x));
        }
    }

    // ----- Create -----
    public sealed record CreateQuinielaCommand(string Nombre, string Reglas, int TorneoId, bool Active)
        : IRequest<Result<QuinielaDto>>;

    public sealed class CreateQuinielaValidator : AbstractValidator<CreateQuinielaCommand>
    {
        public CreateQuinielaValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty().MaximumLength(150).WithMessage("El nombre es requerido (máx. 150).");
            RuleFor(x => x.Reglas).NotEmpty().WithMessage("Las reglas son requeridas.");
            RuleFor(x => x.TorneoId).GreaterThan(0).WithMessage("El torneo es requerido.");
        }
    }

    internal sealed class CreateQuinielaHandler : IRequestHandler<CreateQuinielaCommand, Result<QuinielaDto>>
    {
        private readonly IRepository<QuinielaEntity> _repo;
        private readonly IRepository<Torneo> _torneos;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUser _currentUser;

        public CreateQuinielaHandler(IRepository<QuinielaEntity> repo, IRepository<Torneo> torneos, IUnitOfWork uow, ICurrentUser currentUser)
        { _repo = repo; _torneos = torneos; _uow = uow; _currentUser = currentUser; }

        public async Task<Result<QuinielaDto>> Handle(CreateQuinielaCommand cmd, CancellationToken ct)
        {
            if (!await _torneos.GetDbSet().AsNoTracking().AnyAsync(t => t.Id == cmd.TorneoId, ct))
                return Result.Failure<QuinielaDto>(QuinielaErrors.TorneoNotFound);

            var entity = new QuinielaEntity
            {
                Nombre = cmd.Nombre.Trim(),
                Reglas = cmd.Reglas,
                TorneoId = cmd.TorneoId,
                Active = cmd.Active,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = _currentUser.UserName
            };
            _repo.Insert(entity);
            await _uow.SaveChangesAsync(ct);
            return Result.Success(QuinielaMapper.ToDto(entity));
        }
    }

    // ----- Update -----
    public sealed record UpdateQuinielaCommand(int Id, string Nombre, string Reglas, int TorneoId, bool Active)
        : IRequest<Result<QuinielaDto>>;

    public sealed class UpdateQuinielaValidator : AbstractValidator<UpdateQuinielaCommand>
    {
        public UpdateQuinielaValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty().MaximumLength(150).WithMessage("El nombre es requerido (máx. 150).");
            RuleFor(x => x.Reglas).NotEmpty().WithMessage("Las reglas son requeridas.");
            RuleFor(x => x.TorneoId).GreaterThan(0).WithMessage("El torneo es requerido.");
        }
    }

    internal sealed class UpdateQuinielaHandler : IRequestHandler<UpdateQuinielaCommand, Result<QuinielaDto>>
    {
        private readonly IRepository<QuinielaEntity> _repo;
        private readonly IRepository<Torneo> _torneos;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUser _currentUser;

        public UpdateQuinielaHandler(IRepository<QuinielaEntity> repo, IRepository<Torneo> torneos, IUnitOfWork uow, ICurrentUser currentUser)
        { _repo = repo; _torneos = torneos; _uow = uow; _currentUser = currentUser; }

        public async Task<Result<QuinielaDto>> Handle(UpdateQuinielaCommand cmd, CancellationToken ct)
        {
            var entity = await _repo.GetDbSet().FirstOrDefaultAsync(x => x.Id == cmd.Id, ct);
            if (entity is null) return Result.Failure<QuinielaDto>(QuinielaErrors.NotFound);

            if (!await _torneos.GetDbSet().AsNoTracking().AnyAsync(t => t.Id == cmd.TorneoId, ct))
                return Result.Failure<QuinielaDto>(QuinielaErrors.TorneoNotFound);

            entity.Nombre = cmd.Nombre.Trim();
            entity.Reglas = cmd.Reglas;
            entity.TorneoId = cmd.TorneoId;
            entity.Active = cmd.Active;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = _currentUser.UserName;
            await _uow.SaveChangesAsync(ct);
            return Result.Success(QuinielaMapper.ToDto(entity));
        }
    }

    // ----- Delete (soft delete) -----
    public sealed record DeleteQuinielaCommand(int Id) : IRequest<Result>;

    internal sealed class DeleteQuinielaHandler : IRequestHandler<DeleteQuinielaCommand, Result>
    {
        private readonly IRepository<QuinielaEntity> _repo;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUser _currentUser;

        public DeleteQuinielaHandler(IRepository<QuinielaEntity> repo, IUnitOfWork uow, ICurrentUser currentUser)
        { _repo = repo; _uow = uow; _currentUser = currentUser; }

        public async Task<Result> Handle(DeleteQuinielaCommand cmd, CancellationToken ct)
        {
            var entity = await _repo.GetDbSet().FirstOrDefaultAsync(x => x.Id == cmd.Id, ct);
            if (entity is null) return Result.Failure(QuinielaErrors.NotFound);

            entity.Active = false;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = _currentUser.UserName;
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}

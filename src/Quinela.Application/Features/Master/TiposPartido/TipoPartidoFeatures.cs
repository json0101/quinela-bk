using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quinela.Application.Common.Abstractions;
using Quinela.Application.Common.Results;
using Quinela.Domain.Entities;

namespace Quinela.Application.Features.Master.TiposPartido
{
    public static class TipoPartidoErrors
    {
        public static readonly Error NotFound = Error.NotFound("TipoPartido.NotFound", "No se encontró el tipo de partido.");
        public static readonly Error Duplicate = Error.Conflict("TipoPartido.Duplicate", "Ya existe un tipo de partido con esa descripción.");
        public static readonly Error FaseNotFound = Error.NotFound("TipoPartido.FaseNotFound", "No se encontró la fase indicada.");
    }

    // ----- GetAll -----
    public sealed record GetAllTiposPartidoQuery() : IRequest<Result<List<TipoPartidoDto>>>;

    internal sealed class GetAllTiposPartidoHandler : IRequestHandler<GetAllTiposPartidoQuery, Result<List<TipoPartidoDto>>>
    {
        private readonly IRepository<TipoPartido> _repo;
        public GetAllTiposPartidoHandler(IRepository<TipoPartido> repo) => _repo = repo;

        public async Task<Result<List<TipoPartidoDto>>> Handle(GetAllTiposPartidoQuery request, CancellationToken ct)
        {
            var rows = await _repo.GetDbSet().AsNoTracking().OrderBy(x => x.Descripcion).ToListAsync(ct);
            return Result.Success(rows.Select(MapStatic).ToList());
        }

        private static TipoPartidoDto MapStatic(TipoPartido x) => TipoPartidoMapper.Map(x);
    }

    // ----- GetById -----
    public sealed record GetTipoPartidoByIdQuery(int Id) : IRequest<Result<TipoPartidoDto>>;

    internal sealed class GetTipoPartidoByIdHandler : IRequestHandler<GetTipoPartidoByIdQuery, Result<TipoPartidoDto>>
    {
        private readonly IRepository<TipoPartido> _repo;
        public GetTipoPartidoByIdHandler(IRepository<TipoPartido> repo) => _repo = repo;

        public async Task<Result<TipoPartidoDto>> Handle(GetTipoPartidoByIdQuery request, CancellationToken ct)
        {
            var x = await _repo.GetDbSet().AsNoTracking().FirstOrDefaultAsync(t => t.Id == request.Id, ct);
            if (x is null) return Result.Failure<TipoPartidoDto>(TipoPartidoErrors.NotFound);
            return Result.Success(TipoPartidoMapper.Map(x));
        }
    }

    // ----- Create -----
    public sealed record CreateTipoPartidoCommand(
        string Descripcion, int FaseId, int PtsPartidoVictoria, int PtsPartidoEmpate,
        int PtsQuinelaResultadoExacto, int PtsQuinelaResultadoAcertado, bool Active) : IRequest<Result<TipoPartidoDto>>;

    public sealed class CreateTipoPartidoValidator : AbstractValidator<CreateTipoPartidoCommand>
    {
        public CreateTipoPartidoValidator()
        {
            RuleFor(x => x.Descripcion).NotEmpty().WithMessage("La descripción es requerida.").MaximumLength(120);
            RuleFor(x => x.FaseId).GreaterThan(0).WithMessage("La fase es requerida.");
            RuleFor(x => x.PtsPartidoVictoria).GreaterThanOrEqualTo(0);
            RuleFor(x => x.PtsPartidoEmpate).GreaterThanOrEqualTo(0);
            RuleFor(x => x.PtsQuinelaResultadoExacto).GreaterThanOrEqualTo(0);
            RuleFor(x => x.PtsQuinelaResultadoAcertado).GreaterThanOrEqualTo(0);
        }
    }

    internal sealed class CreateTipoPartidoHandler : IRequestHandler<CreateTipoPartidoCommand, Result<TipoPartidoDto>>
    {
        private readonly IRepository<TipoPartido> _repo;
        private readonly IRepository<Fase> _fases;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUser _currentUser;
        public CreateTipoPartidoHandler(IRepository<TipoPartido> repo, IRepository<Fase> fases, IUnitOfWork uow, ICurrentUser currentUser)
        { _repo = repo; _fases = fases; _uow = uow; _currentUser = currentUser; }

        public async Task<Result<TipoPartidoDto>> Handle(CreateTipoPartidoCommand cmd, CancellationToken ct)
        {
            var faseExists = await _fases.GetDbSet().AsNoTracking().AnyAsync(x => x.Id == cmd.FaseId, ct);
            if (!faseExists) return Result.Failure<TipoPartidoDto>(TipoPartidoErrors.FaseNotFound);

            var descripcion = cmd.Descripcion.Trim();
            var exists = await _repo.GetDbSet().AsNoTracking()
                .AnyAsync(x => x.Descripcion.ToLower() == descripcion.ToLower(), ct);
            if (exists) return Result.Failure<TipoPartidoDto>(TipoPartidoErrors.Duplicate);

            var entity = new TipoPartido
            {
                Descripcion = descripcion,
                FaseId = cmd.FaseId,
                PtsPartidoVictoria = cmd.PtsPartidoVictoria,
                PtsPartidoEmpate = cmd.PtsPartidoEmpate,
                PtsQuinelaResultadoExacto = cmd.PtsQuinelaResultadoExacto,
                PtsQuinelaResultadoAcertado = cmd.PtsQuinelaResultadoAcertado,
                Active = cmd.Active,
                CreatedAt = DateTime.UtcNow, CreatedBy = _currentUser.UserName
            };
            _repo.Insert(entity);
            await _uow.SaveChangesAsync(ct);
            return Result.Success(TipoPartidoMapper.Map(entity));
        }
    }

    // ----- Update -----
    public sealed record UpdateTipoPartidoCommand(
        int Id, string Descripcion, int FaseId, int PtsPartidoVictoria, int PtsPartidoEmpate,
        int PtsQuinelaResultadoExacto, int PtsQuinelaResultadoAcertado, bool Active) : IRequest<Result<TipoPartidoDto>>;

    public sealed class UpdateTipoPartidoValidator : AbstractValidator<UpdateTipoPartidoCommand>
    {
        public UpdateTipoPartidoValidator()
        {
            RuleFor(x => x.Descripcion).NotEmpty().WithMessage("La descripción es requerida.").MaximumLength(120);
            RuleFor(x => x.FaseId).GreaterThan(0).WithMessage("La fase es requerida.");
            RuleFor(x => x.PtsPartidoVictoria).GreaterThanOrEqualTo(0);
            RuleFor(x => x.PtsPartidoEmpate).GreaterThanOrEqualTo(0);
            RuleFor(x => x.PtsQuinelaResultadoExacto).GreaterThanOrEqualTo(0);
            RuleFor(x => x.PtsQuinelaResultadoAcertado).GreaterThanOrEqualTo(0);
        }
    }

    internal sealed class UpdateTipoPartidoHandler : IRequestHandler<UpdateTipoPartidoCommand, Result<TipoPartidoDto>>
    {
        private readonly IRepository<TipoPartido> _repo;
        private readonly IRepository<Fase> _fases;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUser _currentUser;
        public UpdateTipoPartidoHandler(IRepository<TipoPartido> repo, IRepository<Fase> fases, IUnitOfWork uow, ICurrentUser currentUser)
        { _repo = repo; _fases = fases; _uow = uow; _currentUser = currentUser; }

        public async Task<Result<TipoPartidoDto>> Handle(UpdateTipoPartidoCommand cmd, CancellationToken ct)
        {
            var entity = await _repo.GetDbSet().FirstOrDefaultAsync(x => x.Id == cmd.Id, ct);
            if (entity is null) return Result.Failure<TipoPartidoDto>(TipoPartidoErrors.NotFound);

            var faseExists = await _fases.GetDbSet().AsNoTracking().AnyAsync(x => x.Id == cmd.FaseId, ct);
            if (!faseExists) return Result.Failure<TipoPartidoDto>(TipoPartidoErrors.FaseNotFound);

            var descripcion = cmd.Descripcion.Trim();
            var dup = await _repo.GetDbSet().AsNoTracking()
                .AnyAsync(x => x.Id != cmd.Id && x.Descripcion.ToLower() == descripcion.ToLower(), ct);
            if (dup) return Result.Failure<TipoPartidoDto>(TipoPartidoErrors.Duplicate);

            entity.Descripcion = descripcion;
            entity.FaseId = cmd.FaseId;
            entity.PtsPartidoVictoria = cmd.PtsPartidoVictoria;
            entity.PtsPartidoEmpate = cmd.PtsPartidoEmpate;
            entity.PtsQuinelaResultadoExacto = cmd.PtsQuinelaResultadoExacto;
            entity.PtsQuinelaResultadoAcertado = cmd.PtsQuinelaResultadoAcertado;
            entity.Active = cmd.Active;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = _currentUser.UserName;
            await _uow.SaveChangesAsync(ct);
            return Result.Success(TipoPartidoMapper.Map(entity));
        }
    }

    // ----- Delete -----
    public sealed record DeleteTipoPartidoCommand(int Id) : IRequest<Result>;

    internal sealed class DeleteTipoPartidoHandler : IRequestHandler<DeleteTipoPartidoCommand, Result>
    {
        private readonly IRepository<TipoPartido> _repo;
        private readonly IUnitOfWork _uow;
        public DeleteTipoPartidoHandler(IRepository<TipoPartido> repo, IUnitOfWork uow)
        { _repo = repo; _uow = uow; }

        public async Task<Result> Handle(DeleteTipoPartidoCommand cmd, CancellationToken ct)
        {
            var entity = await _repo.GetDbSet().FirstOrDefaultAsync(x => x.Id == cmd.Id, ct);
            if (entity is null) return Result.Failure(TipoPartidoErrors.NotFound);

            _repo.Remove(entity);
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }

    internal static class TipoPartidoMapper
    {
        public static TipoPartidoDto Map(TipoPartido x) => new()
        {
            Id = x.Id,
            Descripcion = x.Descripcion,
            FaseId = x.FaseId,
            PtsPartidoVictoria = x.PtsPartidoVictoria,
            PtsPartidoEmpate = x.PtsPartidoEmpate,
            PtsQuinelaResultadoExacto = x.PtsQuinelaResultadoExacto,
            PtsQuinelaResultadoAcertado = x.PtsQuinelaResultadoAcertado,
            Active = x.Active,
            CreatedAt = x.CreatedAt, CreatedBy = x.CreatedBy, UpdatedAt = x.UpdatedAt, UpdatedBy = x.UpdatedBy
        };
    }
}

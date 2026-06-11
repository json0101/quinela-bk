using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quinela.Application.Common.Abstractions;
using Quinela.Application.Common.Results;
using Quinela.Domain.Entities;

namespace Quinela.Application.Features.Master.Torneos
{
    public class TorneoDto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class TorneoCreateDto { public string Descripcion { get; set; } = string.Empty; public bool Active { get; set; } = true; }
    public class TorneoUpdateDto { public string Descripcion { get; set; } = string.Empty; public bool Active { get; set; } }

    public static class TorneoErrors
    {
        public static readonly Error NotFound = Error.NotFound("Torneo.NotFound", "No se encontró el torneo.");
    }

    internal static class TorneoMapper
    {
        public static TorneoDto ToDto(Torneo x) => new()
        {
            Id = x.Id, Descripcion = x.Descripcion, Active = x.Active,
            CreatedAt = x.CreatedAt, CreatedBy = x.CreatedBy, UpdatedAt = x.UpdatedAt, UpdatedBy = x.UpdatedBy
        };
    }

    public sealed record GetAllTorneosQuery() : IRequest<Result<List<TorneoDto>>>;

    internal sealed class GetAllTorneosHandler : IRequestHandler<GetAllTorneosQuery, Result<List<TorneoDto>>>
    {
        private readonly IRepository<Torneo> _repo;
        public GetAllTorneosHandler(IRepository<Torneo> repo) => _repo = repo;
        public async Task<Result<List<TorneoDto>>> Handle(GetAllTorneosQuery request, CancellationToken ct)
        {
            var rows = await _repo.GetDbSet().AsNoTracking().Where(x => x.Active)
                .OrderBy(x => x.Descripcion).Select(x => TorneoMapper.ToDto(x)).ToListAsync(ct);
            return Result.Success(rows);
        }
    }

    public sealed record CreateTorneoCommand(string Descripcion, bool Active) : IRequest<Result<TorneoDto>>;

    public sealed class CreateTorneoValidator : AbstractValidator<CreateTorneoCommand>
    {
        public CreateTorneoValidator() =>
            RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(200).WithMessage("La descripción es requerida (máx. 200).");
    }

    internal sealed class CreateTorneoHandler : IRequestHandler<CreateTorneoCommand, Result<TorneoDto>>
    {
        private readonly IRepository<Torneo> _repo;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUser _currentUser;
        public CreateTorneoHandler(IRepository<Torneo> repo, IUnitOfWork uow, ICurrentUser currentUser)
        { _repo = repo; _uow = uow; _currentUser = currentUser; }

        public async Task<Result<TorneoDto>> Handle(CreateTorneoCommand cmd, CancellationToken ct)
        {
            var entity = new Torneo
            {
                Descripcion = cmd.Descripcion.Trim(), Active = cmd.Active,
                CreatedAt = DateTime.UtcNow, CreatedBy = _currentUser.UserName
            };
            _repo.Insert(entity);
            await _uow.SaveChangesAsync(ct);
            return Result.Success(TorneoMapper.ToDto(entity));
        }
    }

    public sealed record UpdateTorneoCommand(int Id, string Descripcion, bool Active) : IRequest<Result<TorneoDto>>;

    public sealed class UpdateTorneoValidator : AbstractValidator<UpdateTorneoCommand>
    {
        public UpdateTorneoValidator() =>
            RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(200).WithMessage("La descripción es requerida (máx. 200).");
    }

    internal sealed class UpdateTorneoHandler : IRequestHandler<UpdateTorneoCommand, Result<TorneoDto>>
    {
        private readonly IRepository<Torneo> _repo;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUser _currentUser;
        public UpdateTorneoHandler(IRepository<Torneo> repo, IUnitOfWork uow, ICurrentUser currentUser)
        { _repo = repo; _uow = uow; _currentUser = currentUser; }

        public async Task<Result<TorneoDto>> Handle(UpdateTorneoCommand cmd, CancellationToken ct)
        {
            var entity = await _repo.GetDbSet().FirstOrDefaultAsync(x => x.Id == cmd.Id, ct);
            if (entity is null) return Result.Failure<TorneoDto>(TorneoErrors.NotFound);
            entity.Descripcion = cmd.Descripcion.Trim();
            entity.Active = cmd.Active;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = _currentUser.UserName;
            await _uow.SaveChangesAsync(ct);
            return Result.Success(TorneoMapper.ToDto(entity));
        }
    }

    public sealed record DeleteTorneoCommand(int Id) : IRequest<Result>;

    internal sealed class DeleteTorneoHandler : IRequestHandler<DeleteTorneoCommand, Result>
    {
        private readonly IRepository<Torneo> _repo;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUser _currentUser;
        public DeleteTorneoHandler(IRepository<Torneo> repo, IUnitOfWork uow, ICurrentUser currentUser)
        { _repo = repo; _uow = uow; _currentUser = currentUser; }

        public async Task<Result> Handle(DeleteTorneoCommand cmd, CancellationToken ct)
        {
            var entity = await _repo.GetDbSet().FirstOrDefaultAsync(x => x.Id == cmd.Id, ct);
            if (entity is null) return Result.Failure(TorneoErrors.NotFound);
            entity.Active = false;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = _currentUser.UserName;
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}

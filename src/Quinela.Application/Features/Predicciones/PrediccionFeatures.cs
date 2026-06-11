using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quinela.Application.Common.Abstractions;
using Quinela.Application.Common.Results;
using Quinela.Domain.Entities;
using UserApp.Service.Services.Users;

namespace Quinela.Application.Features.Predicciones
{
    public static class PrediccionErrors
    {
        public static readonly Error NotFound = Error.NotFound("Prediccion.NotFound", "No se encontró la predicción.");
        public static readonly Error PartidoNotFound = Error.NotFound("Prediccion.PartidoNotFound", "No se encontró el partido.");
        public static readonly Error QuinielaNotFound = Error.NotFound("Prediccion.QuinielaNotFound", "No se encontró la quiniela.");
        public static readonly Error UserNotFound = Error.Validation("Prediccion.UserNotFound", "El usuario no existe en UserApp.");
        public static readonly Error PartidoCerrado = Error.Conflict("Prediccion.PartidoCerrado", "El partido ya no admite predicciones.");
    }

    internal static class PrediccionMapper
    {
        public static PrediccionDto ToDto(Prediccion x) => new()
        {
            Id = x.Id,
            QuinielaId = x.QuinielaId,
            PartidoId = x.PartidoId,
            Team1Resultado = x.Team1Resultado,
            Team2Resultado = x.Team2Resultado,
            Username = x.Username,
            Active = x.Active,
            CreatedAt = x.CreatedAt,
            CreatedBy = x.CreatedBy,
            UpdatedAt = x.UpdatedAt,
            UpdatedBy = x.UpdatedBy
        };
    }

    // ----- GetAll por quiniela (solo activas) -----
    public sealed record GetAllPrediccionesQuery(int QuinielaId) : IRequest<Result<List<PrediccionDto>>>;

    internal sealed class GetAllPrediccionesHandler : IRequestHandler<GetAllPrediccionesQuery, Result<List<PrediccionDto>>>
    {
        private readonly IRepository<Prediccion> _repo;
        public GetAllPrediccionesHandler(IRepository<Prediccion> repo) => _repo = repo;

        public async Task<Result<List<PrediccionDto>>> Handle(GetAllPrediccionesQuery request, CancellationToken ct)
        {
            var rows = await _repo.GetDbSet().AsNoTracking()
                .Where(x => x.Active && x.QuinielaId == request.QuinielaId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => PrediccionMapper.ToDto(x))
                .ToListAsync(ct);
            return Result.Success(rows);
        }
    }

    // ----- GetById -----
    public sealed record GetPrediccionByIdQuery(int Id) : IRequest<Result<PrediccionDto>>;

    internal sealed class GetPrediccionByIdHandler : IRequestHandler<GetPrediccionByIdQuery, Result<PrediccionDto>>
    {
        private readonly IRepository<Prediccion> _repo;
        public GetPrediccionByIdHandler(IRepository<Prediccion> repo) => _repo = repo;

        public async Task<Result<PrediccionDto>> Handle(GetPrediccionByIdQuery request, CancellationToken ct)
        {
            var x = await _repo.GetDbSet().AsNoTracking().FirstOrDefaultAsync(p => p.Id == request.Id, ct);
            if (x is null) return Result.Failure<PrediccionDto>(PrediccionErrors.NotFound);
            return Result.Success(PrediccionMapper.ToDto(x));
        }
    }

    // ----- Create -----
    public sealed record CreatePrediccionCommand(int QuinielaId, int PartidoId, int? Team1Resultado, int? Team2Resultado, bool Active)
        : IRequest<Result<PrediccionDto>>;

    public sealed class CreatePrediccionValidator : AbstractValidator<CreatePrediccionCommand>
    {
        public CreatePrediccionValidator()
        {
            RuleFor(x => x.QuinielaId).GreaterThan(0).WithMessage("La quiniela es requerida.");
            RuleFor(x => x.PartidoId).GreaterThan(0).WithMessage("El partido es requerido.");
            RuleFor(x => x.Team1Resultado).GreaterThanOrEqualTo(0).WithMessage("El resultado del equipo 1 no puede ser negativo.");
            RuleFor(x => x.Team2Resultado).GreaterThanOrEqualTo(0).WithMessage("El resultado del equipo 2 no puede ser negativo.");
        }
    }

    internal sealed class CreatePrediccionHandler : IRequestHandler<CreatePrediccionCommand, Result<PrediccionDto>>
    {
        private readonly IRepository<Prediccion> _repo;
        private readonly IRepository<Partido> _partidos;
        private readonly IRepository<Quiniela> _quinielas;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUser _currentUser;
        private readonly IUserService _userService;

        public CreatePrediccionHandler(IRepository<Prediccion> repo, IRepository<Partido> partidos, IRepository<Quiniela> quinielas,
            IUnitOfWork uow, ICurrentUser currentUser, IUserService userService)
        { _repo = repo; _partidos = partidos; _quinielas = quinielas; _uow = uow; _currentUser = currentUser; _userService = userService; }

        public async Task<Result<PrediccionDto>> Handle(CreatePrediccionCommand cmd, CancellationToken ct)
        {
            if (!await _quinielas.GetDbSet().AsNoTracking().AnyAsync(q => q.Id == cmd.QuinielaId, ct))
                return Result.Failure<PrediccionDto>(PrediccionErrors.QuinielaNotFound);

            var partido = await _partidos.GetDbSet().AsNoTracking().FirstOrDefaultAsync(x => x.Id == cmd.PartidoId, ct);
            if (partido is null) return Result.Failure<PrediccionDto>(PrediccionErrors.PartidoNotFound);
            if (partido.Estado != 'P') return Result.Failure<PrediccionDto>(PrediccionErrors.PartidoCerrado);

            var username = _currentUser.UserName;
            if (!UserExists(username)) return Result.Failure<PrediccionDto>(PrediccionErrors.UserNotFound);

            var entity = new Prediccion
            {
                QuinielaId = cmd.QuinielaId,
                PartidoId = cmd.PartidoId,
                Team1Resultado = cmd.Team1Resultado,
                Team2Resultado = cmd.Team2Resultado,
                Username = username,
                Active = cmd.Active,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = username
            };
            _repo.Insert(entity);
            await _uow.SaveChangesAsync(ct);

            return Result.Success(PrediccionMapper.ToDto(entity));
        }

        private bool UserExists(string username) => _userService.GetResume().Any(u => u.UserName == username);
    }

    // ----- Update -----
    public sealed record UpdatePrediccionCommand(int Id, int QuinielaId, int PartidoId, int? Team1Resultado, int? Team2Resultado, bool Active)
        : IRequest<Result<PrediccionDto>>;

    public sealed class UpdatePrediccionValidator : AbstractValidator<UpdatePrediccionCommand>
    {
        public UpdatePrediccionValidator()
        {
            RuleFor(x => x.QuinielaId).GreaterThan(0).WithMessage("La quiniela es requerida.");
            RuleFor(x => x.PartidoId).GreaterThan(0).WithMessage("El partido es requerido.");
            RuleFor(x => x.Team1Resultado).GreaterThanOrEqualTo(0).WithMessage("El resultado del equipo 1 no puede ser negativo.");
            RuleFor(x => x.Team2Resultado).GreaterThanOrEqualTo(0).WithMessage("El resultado del equipo 2 no puede ser negativo.");
        }
    }

    internal sealed class UpdatePrediccionHandler : IRequestHandler<UpdatePrediccionCommand, Result<PrediccionDto>>
    {
        private readonly IRepository<Prediccion> _repo;
        private readonly IRepository<Partido> _partidos;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUser _currentUser;

        public UpdatePrediccionHandler(IRepository<Prediccion> repo, IRepository<Partido> partidos,
            IUnitOfWork uow, ICurrentUser currentUser)
        { _repo = repo; _partidos = partidos; _uow = uow; _currentUser = currentUser; }

        public async Task<Result<PrediccionDto>> Handle(UpdatePrediccionCommand cmd, CancellationToken ct)
        {
            var entity = await _repo.GetDbSet().FirstOrDefaultAsync(x => x.Id == cmd.Id, ct);
            if (entity is null) return Result.Failure<PrediccionDto>(PrediccionErrors.NotFound);

            var partido = await _partidos.GetDbSet().AsNoTracking().FirstOrDefaultAsync(x => x.Id == cmd.PartidoId, ct);
            if (partido is null) return Result.Failure<PrediccionDto>(PrediccionErrors.PartidoNotFound);
            if (partido.Estado != 'P') return Result.Failure<PrediccionDto>(PrediccionErrors.PartidoCerrado);

            entity.QuinielaId = cmd.QuinielaId;
            entity.PartidoId = cmd.PartidoId;
            entity.Team1Resultado = cmd.Team1Resultado;
            entity.Team2Resultado = cmd.Team2Resultado;
            entity.Active = cmd.Active;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = _currentUser.UserName;
            await _uow.SaveChangesAsync(ct);

            return Result.Success(PrediccionMapper.ToDto(entity));
        }
    }

    // ----- Upsert (crea o actualiza la predicción del usuario para un partido en una quiniela) -----
    public sealed record UpsertPrediccionCommand(int QuinielaId, int PartidoId, int? Team1Resultado, int? Team2Resultado)
        : IRequest<Result<PrediccionDto>>;

    public sealed class UpsertPrediccionValidator : AbstractValidator<UpsertPrediccionCommand>
    {
        public UpsertPrediccionValidator()
        {
            RuleFor(x => x.QuinielaId).GreaterThan(0).WithMessage("La quiniela es requerida.");
            RuleFor(x => x.PartidoId).GreaterThan(0).WithMessage("El partido es requerido.");
            RuleFor(x => x.Team1Resultado).GreaterThanOrEqualTo(0).WithMessage("El resultado del equipo 1 no puede ser negativo.");
            RuleFor(x => x.Team2Resultado).GreaterThanOrEqualTo(0).WithMessage("El resultado del equipo 2 no puede ser negativo.");
            RuleFor(x => x).Must(c => c.Team1Resultado.HasValue || c.Team2Resultado.HasValue)
                .WithMessage("Ingresa al menos un resultado.");
        }
    }

    internal sealed class UpsertPrediccionHandler : IRequestHandler<UpsertPrediccionCommand, Result<PrediccionDto>>
    {
        private readonly IRepository<Prediccion> _repo;
        private readonly IRepository<Partido> _partidos;
        private readonly IRepository<Quiniela> _quinielas;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUser _currentUser;
        private readonly IUserService _userService;

        public UpsertPrediccionHandler(IRepository<Prediccion> repo, IRepository<Partido> partidos, IRepository<Quiniela> quinielas,
            IUnitOfWork uow, ICurrentUser currentUser, IUserService userService)
        { _repo = repo; _partidos = partidos; _quinielas = quinielas; _uow = uow; _currentUser = currentUser; _userService = userService; }

        public async Task<Result<PrediccionDto>> Handle(UpsertPrediccionCommand cmd, CancellationToken ct)
        {
            if (!await _quinielas.GetDbSet().AsNoTracking().AnyAsync(q => q.Id == cmd.QuinielaId, ct))
                return Result.Failure<PrediccionDto>(PrediccionErrors.QuinielaNotFound);

            var partido = await _partidos.GetDbSet().AsNoTracking().FirstOrDefaultAsync(x => x.Id == cmd.PartidoId, ct);
            if (partido is null) return Result.Failure<PrediccionDto>(PrediccionErrors.PartidoNotFound);
            if (partido.Estado != 'P') return Result.Failure<PrediccionDto>(PrediccionErrors.PartidoCerrado);

            var username = _currentUser.UserName;
            if (!_userService.GetResume().Any(u => u.UserName == username))
                return Result.Failure<PrediccionDto>(PrediccionErrors.UserNotFound);

            var entity = await _repo.GetDbSet().FirstOrDefaultAsync(
                x => x.QuinielaId == cmd.QuinielaId && x.PartidoId == cmd.PartidoId && x.Username == username && x.Active, ct);

            if (entity is not null)
            {
                Aplicar(entity, cmd, username);
                await _uow.SaveChangesAsync(ct);
                return Result.Success(PrediccionMapper.ToDto(entity));
            }

            // Crear. Si dos peticiones crean a la vez, el índice único parcial
            // (quiniela+partido+usuario, activas) rechaza la segunda: la recuperamos y actualizamos.
            var nueva = new Prediccion
            {
                QuinielaId = cmd.QuinielaId,
                PartidoId = cmd.PartidoId,
                Team1Resultado = cmd.Team1Resultado,
                Team2Resultado = cmd.Team2Resultado,
                Username = username,
                Active = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = username
            };
            _repo.Insert(nueva);

            try
            {
                await _uow.SaveChangesAsync(ct);
                return Result.Success(PrediccionMapper.ToDto(nueva));
            }
            catch (DbUpdateException)
            {
                _repo.Detach(nueva);
                var existente = await _repo.GetDbSet().FirstOrDefaultAsync(
                    x => x.QuinielaId == cmd.QuinielaId && x.PartidoId == cmd.PartidoId && x.Username == username && x.Active, ct);
                if (existente is null) throw;

                Aplicar(existente, cmd, username);
                await _uow.SaveChangesAsync(ct);
                return Result.Success(PrediccionMapper.ToDto(existente));
            }
        }

        private static void Aplicar(Prediccion entity, UpsertPrediccionCommand cmd, string username)
        {
            entity.Team1Resultado = cmd.Team1Resultado;
            entity.Team2Resultado = cmd.Team2Resultado;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = username;
        }
    }

    // ----- Delete (soft delete: solo apaga Active) -----
    public sealed record DeletePrediccionCommand(int Id) : IRequest<Result>;

    internal sealed class DeletePrediccionHandler : IRequestHandler<DeletePrediccionCommand, Result>
    {
        private readonly IRepository<Prediccion> _repo;
        private readonly IRepository<Partido> _partidos;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUser _currentUser;

        public DeletePrediccionHandler(IRepository<Prediccion> repo, IRepository<Partido> partidos,
            IUnitOfWork uow, ICurrentUser currentUser)
        { _repo = repo; _partidos = partidos; _uow = uow; _currentUser = currentUser; }

        public async Task<Result> Handle(DeletePrediccionCommand cmd, CancellationToken ct)
        {
            var entity = await _repo.GetDbSet().FirstOrDefaultAsync(x => x.Id == cmd.Id, ct);
            if (entity is null) return Result.Failure(PrediccionErrors.NotFound);

            var estado = await _partidos.GetDbSet().AsNoTracking()
                .Where(x => x.Id == entity.PartidoId).Select(x => (char?)x.Estado).FirstOrDefaultAsync(ct);
            if (estado is not null && estado != 'P') return Result.Failure(PrediccionErrors.PartidoCerrado);

            entity.Active = false;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = _currentUser.UserName;
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}

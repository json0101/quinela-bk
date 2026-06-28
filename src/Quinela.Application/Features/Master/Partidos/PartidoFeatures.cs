using System.Linq.Expressions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quinela.Application.Common.Abstractions;
using Quinela.Application.Common.Results;
using Quinela.Domain.Entities;

namespace Quinela.Application.Features.Master.Partidos
{
    public static class PartidoAdminErrors
    {
        public static readonly Error NotFound = Error.NotFound("Partido.NotFound", "No se encontró el partido.");
        public static readonly Error TorneoNotFound = Error.NotFound("Partido.TorneoNotFound", "No se encontró el torneo.");
        public static readonly Error GrupoInvalido = Error.NotFound("Partido.GrupoInvalido", "El grupo no existe o no pertenece al torneo.");
        public static readonly Error EquipoLocalInvalido = Error.NotFound("Partido.EquipoLocalInvalido", "El equipo local no existe o no pertenece al torneo.");
        public static readonly Error EquipoVisitanteInvalido = Error.NotFound("Partido.EquipoVisitanteInvalido", "El equipo visitante no existe o no pertenece al torneo.");
        public static readonly Error TipoPartidoNotFound = Error.NotFound("Partido.TipoPartidoNotFound", "No se encontró el tipo de partido.");
        public static readonly Error FaseNotFound = Error.NotFound("Partido.FaseNotFound", "No se encontró la fase.");
        public static readonly Error FaseNoEliminatoria = Error.Validation("Partido.FaseNoEliminatoria", "Un partido 'por definirse' solo puede ser de fase eliminatoria.");
        public static readonly Error EquipoGanadorInvalido = Error.NotFound("Partido.EquipoGanadorInvalido", "El equipo ganador no existe o no pertenece al torneo.");
        public static readonly Error PartidoGanadorInvalido = Error.NotFound("Partido.PartidoGanadorInvalido", "Un partido del árbol de eliminatoria no existe.");
        public static readonly Error MismosEquipos = Error.Validation("Partido.MismosEquipos", "El equipo local y el visitante no pueden ser el mismo.");
    }

    // Proyección reutilizable a DTO (resuelve nombres de las relaciones).
    internal static class PartidoAdminProjection
    {
        public static System.Linq.Expressions.Expression<System.Func<Partido, PartidoAdminDto>> ToDto =>
            p => new PartidoAdminDto
            {
                Id = p.Id,
                FechaPartido = p.FechaPartido,
                TorneoId = p.TorneoId,
                Torneo = p.Torneo.Descripcion,
                GrupoId = p.GrupoId ?? 0,
                Grupo = p.Grupo != null ? p.Grupo.Nombre : "",
                FaseId = p.FaseId,
                Fase = p.Fase.Descripcion,
                EquipoLocalId = p.EquipoLocalId ?? 0,
                EquipoLocal = p.EquipoLocal != null ? p.EquipoLocal.Nombre : "",
                EquipoVisitanteId = p.EquipoVisitanteId ?? 0,
                EquipoVisitante = p.EquipoVisitante != null ? p.EquipoVisitante.Nombre : "",
                EquipoGanadorId = p.EquipoGanadorId,
                EquipoGanador = p.EquipoGanador != null ? p.EquipoGanador.Nombre : null,
                TipoPartidoId = p.TipoPartidoId,
                TipoPartido = p.TipoPartido.Descripcion,
                ResultadoLocal = p.ResultadoLocalId,
                ResultadoVisitante = p.ResultadoVisitanteId,
                PtsLocal = p.PtsLocal,
                PtsVisitante = p.PtsVisitante,
                Estado = p.Estado,
                PartidoIdApi = p.PartidoIdApi,
                PorDefinirse = p.PorDefinirse,
                AplicaDefinicionPenales = p.AplicaDefinicionPenales,
                PartidoSeDefiniraEnPenales = p.PartidoSeDefiniraEnPenales,
                PenalesAnotadosLocal = p.PenalesAnotadosLocal,
                PenalesAnotadosVisitante = p.PenalesAnotadosVisitante,
                PartidoGanadorLocalId = p.PartidoGanadorLocalId,
                PartidoGanadorVisitanteId = p.PartidoGanadorVisitanteId,
                Active = p.Active,
                CreatedAt = p.CreatedAt,
                CreatedBy = p.CreatedBy,
                UpdatedAt = p.UpdatedAt,
                UpdatedBy = p.UpdatedBy
            };
    }

    // ----- GetAll (opcionalmente filtrado por torneo) -----
    public sealed record GetAllPartidosQuery(int? TorneoId) : IRequest<Result<List<PartidoAdminDto>>>;

    internal sealed class GetAllPartidosHandler : IRequestHandler<GetAllPartidosQuery, Result<List<PartidoAdminDto>>>
    {
        private readonly IRepository<Partido> _repo;
        public GetAllPartidosHandler(IRepository<Partido> repo) => _repo = repo;

        public async Task<Result<List<PartidoAdminDto>>> Handle(GetAllPartidosQuery request, CancellationToken ct)
        {
            var rows = await _repo.GetDbSet().AsNoTracking()
                .Where(p => !request.TorneoId.HasValue || p.TorneoId == request.TorneoId.Value)
                .OrderBy(p => p.FechaPartido).ThenBy(p => p.Id)
                .Select(PartidoAdminProjection.ToDto)
                .ToListAsync(ct);
            return Result.Success(rows);
        }
    }

    // ----- GetById -----
    public sealed record GetPartidoByIdQuery(int Id) : IRequest<Result<PartidoAdminDto>>;

    internal sealed class GetPartidoByIdHandler : IRequestHandler<GetPartidoByIdQuery, Result<PartidoAdminDto>>
    {
        private readonly IRepository<Partido> _repo;
        public GetPartidoByIdHandler(IRepository<Partido> repo) => _repo = repo;

        public async Task<Result<PartidoAdminDto>> Handle(GetPartidoByIdQuery request, CancellationToken ct)
        {
            var x = await _repo.GetDbSet().AsNoTracking()
                .Where(p => p.Id == request.Id)
                .Select(PartidoAdminProjection.ToDto)
                .FirstOrDefaultAsync(ct);
            if (x is null) return Result.Failure<PartidoAdminDto>(PartidoAdminErrors.NotFound);
            return Result.Success(x);
        }
    }

    // ----- Create -----
    public sealed record CreatePartidoCommand(
        DateTime FechaPartido, int TorneoId, int? GrupoId, int FaseId, int? EquipoLocalId, int? EquipoVisitanteId, int TipoPartidoId,
        char Estado, int? ResultadoLocal, int? ResultadoVisitante, string? PartidoIdApi, bool Active,
        bool? PartidoSeDefiniraEnPenales, int? PenalesAnotadosLocal, int? PenalesAnotadosVisitante,
        int? EquipoGanadorId, int? PartidoGanadorLocalId, int? PartidoGanadorVisitanteId, bool PorDefinirse,
        bool AplicaDefinicionPenales)
        : IRequest<Result<PartidoAdminDto>>;

    public sealed class CreatePartidoValidator : AbstractValidator<CreatePartidoCommand>
    {
        public CreatePartidoValidator()
        {
            RuleFor(x => x.FechaPartido).NotEmpty().WithMessage("La fecha del partido es requerida.");
            RuleFor(x => x.TorneoId).GreaterThan(0).WithMessage("El torneo es requerido.");
            RuleFor(x => x.FaseId).GreaterThan(0).WithMessage("La fase es requerida.");
            RuleFor(x => x.TipoPartidoId).GreaterThan(0).WithMessage("El tipo de partido es requerido.");
            // Un partido normal exige grupo, equipos y reglas de estado/goles.
            // Uno "por definirse" se arma del árbol: esos campos no aplican.
            When(x => !x.PorDefinirse, () =>
            {
                // El grupo es opcional (se puede dejar sin grupo); los equipos sí se requieren.
                RuleFor(x => x.EquipoLocalId).Must(v => v is > 0).WithMessage("El equipo local es requerido.");
                RuleFor(x => x.EquipoVisitanteId).Must(v => v is > 0).WithMessage("El equipo visitante es requerido.");
                PartidoEstadoHelper.AplicarReglas(this, x => x.Estado, x => x.ResultadoLocal, x => x.ResultadoVisitante);
            });
        }
    }

    internal sealed class CreatePartidoHandler : IRequestHandler<CreatePartidoCommand, Result<PartidoAdminDto>>
    {
        private readonly IRepository<Partido> _repo;
        private readonly IRepository<TipoPartido> _tipos;
        private readonly PartidoRelacionesValidator _rel;
        private readonly IRankingService _ranking;
        private readonly Eliminatoria.IDistributionEliminatoryWorldCup2026 _eliminatoria;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUser _currentUser;
        public CreatePartidoHandler(IRepository<Partido> repo, IRepository<TipoPartido> tipos,
            PartidoRelacionesValidator rel, IRankingService ranking,
            Eliminatoria.IDistributionEliminatoryWorldCup2026 eliminatoria, IUnitOfWork uow, ICurrentUser currentUser)
        { _repo = repo; _tipos = tipos; _rel = rel; _ranking = ranking; _eliminatoria = eliminatoria; _uow = uow; _currentUser = currentUser; }

        public async Task<Result<PartidoAdminDto>> Handle(CreatePartidoCommand cmd, CancellationToken ct)
        {
            var check = await _rel.ValidarAsync(cmd.TorneoId, cmd.GrupoId, cmd.FaseId, cmd.EquipoLocalId, cmd.EquipoVisitanteId,
                cmd.TipoPartidoId, cmd.EquipoGanadorId, cmd.PartidoGanadorLocalId, cmd.PartidoGanadorVisitanteId, cmd.PorDefinirse, ct);
            if (check is not null) return Result.Failure<PartidoAdminDto>(check);

            Partido entity;
            if (cmd.PorDefinirse)
            {
                // "Por definirse": solo árbol + fecha + torneo (la fase debe ser eliminatoria).
                entity = PartidoBuilder
                    .PorDefinirse(cmd.FaseId, cmd.TorneoId, cmd.FechaPartido, cmd.PartidoGanadorLocalId, cmd.PartidoGanadorVisitanteId)
                    .ConAuditoria(_currentUser.UserName)
                    .Build();
                entity.TipoPartidoId = cmd.TipoPartidoId;
                entity.AplicaDefinicionPenales = cmd.AplicaDefinicionPenales;
            }
            else
            {
                var tipo = await _tipos.GetDbSet().AsNoTracking().FirstAsync(t => t.Id == cmd.TipoPartidoId, ct);
                // La ficha base se arma igual en cualquier fase; en eliminatoria se agrega la definición.
                var builder = PartidoBuilder.DeFase(cmd.FaseId)
                    .ConFicha(cmd.FechaPartido, cmd.TorneoId, cmd.GrupoId, cmd.EquipoLocalId!.Value, cmd.EquipoVisitanteId!.Value, cmd.TipoPartidoId, cmd.PartidoIdApi, cmd.Active)
                    .ConEstado(cmd.Estado, cmd.ResultadoLocal, cmd.ResultadoVisitante, tipo)
                    .ConAuditoria(_currentUser.UserName);

                if (FasesConocidas.Tipo(cmd.FaseId) == TipoFase.Eliminatoria)
                    builder.ConDefinicionEliminatoria(new DefinicionEliminatoria(
                        cmd.AplicaDefinicionPenales, cmd.PartidoSeDefiniraEnPenales, cmd.PenalesAnotadosLocal, cmd.PenalesAnotadosVisitante,
                        cmd.EquipoGanadorId, cmd.PartidoGanadorLocalId, cmd.PartidoGanadorVisitanteId));

                entity = builder.Build();
            }

            _repo.Insert(entity);
            await _uow.SaveChangesAsync(ct);

            // Si nace jugado (E/T), recalcular posiciones + ranking del torneo.
            if (entity.Estado != 'P')
                await _ranking.RecalcularAsync(entity.TorneoId, ct);
            // Crear un partido puede afectar el árbol (es una referencia nueva) -> recalcular llaves.
            await _eliminatoria.RecalcularAsync(entity.TorneoId, ct);

            var dto = await _repo.GetDbSet().AsNoTracking()
                .Where(p => p.Id == entity.Id).Select(PartidoAdminProjection.ToDto).FirstAsync(ct);
            return Result.Success(dto);
        }
    }

    // ----- Update (edición manual completa: ficha + estado + goles, y recalcula el ranking) -----
    public sealed record UpdatePartidoCommand(
        int Id, DateTime FechaPartido, int TorneoId, int? GrupoId, int FaseId, int? EquipoLocalId, int? EquipoVisitanteId, int TipoPartidoId,
        char Estado, int? ResultadoLocal, int? ResultadoVisitante, string? PartidoIdApi, bool Active,
        bool? PartidoSeDefiniraEnPenales, int? PenalesAnotadosLocal, int? PenalesAnotadosVisitante,
        int? EquipoGanadorId, int? PartidoGanadorLocalId, int? PartidoGanadorVisitanteId, bool PorDefinirse,
        bool AplicaDefinicionPenales)
        : IRequest<Result<PartidoAdminDto>>;

    public sealed class UpdatePartidoValidator : AbstractValidator<UpdatePartidoCommand>
    {
        public UpdatePartidoValidator()
        {
            RuleFor(x => x.FechaPartido).NotEmpty().WithMessage("La fecha del partido es requerida.");
            RuleFor(x => x.TorneoId).GreaterThan(0).WithMessage("El torneo es requerido.");
            RuleFor(x => x.FaseId).GreaterThan(0).WithMessage("La fase es requerida.");
            RuleFor(x => x.TipoPartidoId).GreaterThan(0).WithMessage("El tipo de partido es requerido.");
            When(x => !x.PorDefinirse, () =>
            {
                // El grupo es opcional (se puede dejar sin grupo); los equipos sí se requieren.
                RuleFor(x => x.EquipoLocalId).Must(v => v is > 0).WithMessage("El equipo local es requerido.");
                RuleFor(x => x.EquipoVisitanteId).Must(v => v is > 0).WithMessage("El equipo visitante es requerido.");
                PartidoEstadoHelper.AplicarReglas(this, x => x.Estado, x => x.ResultadoLocal, x => x.ResultadoVisitante);
            });
        }
    }

    internal sealed class UpdatePartidoHandler : IRequestHandler<UpdatePartidoCommand, Result<PartidoAdminDto>>
    {
        private readonly IRepository<Partido> _repo;
        private readonly IRepository<TipoPartido> _tipos;
        private readonly PartidoRelacionesValidator _rel;
        private readonly IRankingService _ranking;
        private readonly Eliminatoria.IDistributionEliminatoryWorldCup2026 _eliminatoria;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUser _currentUser;
        public UpdatePartidoHandler(IRepository<Partido> repo, IRepository<TipoPartido> tipos,
            PartidoRelacionesValidator rel, IRankingService ranking,
            Eliminatoria.IDistributionEliminatoryWorldCup2026 eliminatoria, IUnitOfWork uow, ICurrentUser currentUser)
        { _repo = repo; _tipos = tipos; _rel = rel; _ranking = ranking; _eliminatoria = eliminatoria; _uow = uow; _currentUser = currentUser; }

        public async Task<Result<PartidoAdminDto>> Handle(UpdatePartidoCommand cmd, CancellationToken ct)
        {
            var entity = await _repo.GetDbSet().FirstOrDefaultAsync(x => x.Id == cmd.Id, ct);
            if (entity is null) return Result.Failure<PartidoAdminDto>(PartidoAdminErrors.NotFound);

            var check = await _rel.ValidarAsync(cmd.TorneoId, cmd.GrupoId, cmd.FaseId, cmd.EquipoLocalId, cmd.EquipoVisitanteId,
                cmd.TipoPartidoId, cmd.EquipoGanadorId, cmd.PartidoGanadorLocalId, cmd.PartidoGanadorVisitanteId, cmd.PorDefinirse, ct);
            if (check is not null) return Result.Failure<PartidoAdminDto>(check);

            var tipo = await _tipos.GetDbSet().AsNoTracking().FirstAsync(t => t.Id == cmd.TipoPartidoId, ct);
            var torneoAnterior = entity.TorneoId;

            entity.FechaPartido = DateTime.SpecifyKind(cmd.FechaPartido, DateTimeKind.Utc);
            entity.TorneoId = cmd.TorneoId;
            entity.FaseId = cmd.FaseId;
            entity.TipoPartidoId = cmd.TipoPartidoId;
            entity.Active = cmd.Active;
            entity.PorDefinirse = cmd.PorDefinirse;

            if (cmd.PorDefinirse)
            {
                // Misma lógica del builder "por definirse": solo árbol + fecha + torneo.
                entity.GrupoId = null;
                entity.EquipoLocalId = null;
                entity.EquipoVisitanteId = null;
                entity.PartidoIdApi = null;
                PartidoEstadoHelper.Aplicar(entity, 'P', null, null, tipo); // estado P, limpia marcador
                entity.AplicaDefinicionPenales = cmd.AplicaDefinicionPenales;
                entity.PartidoSeDefiniraEnPenales = null;
                entity.PenalesAnotadosLocal = null;
                entity.PenalesAnotadosVisitante = null;
                entity.EquipoGanadorId = null;
                entity.PartidoGanadorLocalId = cmd.PartidoGanadorLocalId;
                entity.PartidoGanadorVisitanteId = cmd.PartidoGanadorVisitanteId;
            }
            else
            {
                entity.GrupoId = cmd.GrupoId;
                entity.EquipoLocalId = cmd.EquipoLocalId;
                entity.EquipoVisitanteId = cmd.EquipoVisitanteId;
                entity.PartidoIdApi = cmd.PartidoIdApi;
                // Estado + goles + puntos del partido (P limpia el marcador; E/T lo calcula).
                PartidoEstadoHelper.Aplicar(entity, cmd.Estado, cmd.ResultadoLocal, cmd.ResultadoVisitante, tipo);
                // Reescribe la definición de eliminatoria según la fase (en grupos la limpia).
                if (FasesConocidas.Tipo(cmd.FaseId) == TipoFase.Eliminatoria)
                    DefinicionEliminatoriaAplicador.Aplicar(entity, new DefinicionEliminatoria(
                        cmd.AplicaDefinicionPenales, cmd.PartidoSeDefiniraEnPenales, cmd.PenalesAnotadosLocal, cmd.PenalesAnotadosVisitante,
                        cmd.EquipoGanadorId, cmd.PartidoGanadorLocalId, cmd.PartidoGanadorVisitanteId));
                else
                    DefinicionEliminatoriaAplicador.Limpiar(entity);
            }
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = _currentUser.UserName;
            await _uow.SaveChangesAsync(ct);

            // Edición manual: recalcular posiciones + ranking. Si cambió de torneo, recalcular ambos.
            await _ranking.RecalcularAsync(entity.TorneoId, ct);
            if (torneoAnterior != entity.TorneoId)
                await _ranking.RecalcularAsync(torneoAnterior, ct);

            // Al cambiar un resultado se reacomoda el cuadro de eliminatoria (equipos + ganadores + árbol).
            await _eliminatoria.RecalcularAsync(entity.TorneoId, ct);
            if (torneoAnterior != entity.TorneoId)
                await _eliminatoria.RecalcularAsync(torneoAnterior, ct);

            var dto = await _repo.GetDbSet().AsNoTracking()
                .Where(p => p.Id == entity.Id).Select(PartidoAdminProjection.ToDto).FirstAsync(ct);
            return Result.Success(dto);
        }
    }

    // ----- Delete (soft delete: hay predicciones que referencian al partido) -----
    public sealed record DeletePartidoCommand(int Id) : IRequest<Result>;

    internal sealed class DeletePartidoHandler : IRequestHandler<DeletePartidoCommand, Result>
    {
        private readonly IRepository<Partido> _repo;
        private readonly IRankingService _ranking;
        private readonly Eliminatoria.IDistributionEliminatoryWorldCup2026 _eliminatoria;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUser _currentUser;
        public DeletePartidoHandler(IRepository<Partido> repo, IRankingService ranking,
            Eliminatoria.IDistributionEliminatoryWorldCup2026 eliminatoria, IUnitOfWork uow, ICurrentUser currentUser)
        { _repo = repo; _ranking = ranking; _eliminatoria = eliminatoria; _uow = uow; _currentUser = currentUser; }

        public async Task<Result> Handle(DeletePartidoCommand cmd, CancellationToken ct)
        {
            var entity = await _repo.GetDbSet().FirstOrDefaultAsync(x => x.Id == cmd.Id, ct);
            if (entity is null) return Result.Failure(PartidoAdminErrors.NotFound);

            entity.Active = false;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = _currentUser.UserName;
            await _uow.SaveChangesAsync(ct);

            // Al eliminar un partido se recalcula: posiciones/ranking y el árbol de eliminatoria
            // (si era un partido de referencia, su dependiente queda sin ese equipo -> null).
            await _ranking.RecalcularAsync(entity.TorneoId, ct);
            await _eliminatoria.RecalcularAsync(entity.TorneoId, ct);
            return Result.Success();
        }
    }

    // Aplica el estado, los goles y los puntos del partido, y aporta las reglas de validación.
    // Misma lógica que el cambio de estado del calendario: 'P' limpia el marcador; 'E'/'T' lo calcula.
    internal static class PartidoEstadoHelper
    {
        public static void Aplicar(Partido p, char estado, int? rl, int? rv, TipoPartido tipo)
        {
            p.Estado = estado;
            if (estado == 'P' || rl is null || rv is null)
            {
                p.ResultadoLocalId = null;
                p.ResultadoVisitanteId = null;
                p.PtsLocal = null;
                p.PtsVisitante = null;
                return;
            }

            var signo = Math.Sign(rl.Value - rv.Value);
            p.ResultadoLocalId = rl.Value;
            p.ResultadoVisitanteId = rv.Value;
            p.PtsLocal = signo > 0 ? tipo.PtsPartidoVictoria : signo == 0 ? tipo.PtsPartidoEmpate : 0;
            p.PtsVisitante = signo < 0 ? tipo.PtsPartidoVictoria : signo == 0 ? tipo.PtsPartidoEmpate : 0;
        }

        public static void AplicarReglas<T>(
            AbstractValidator<T> v,
            Expression<Func<T, char>> estado,
            Expression<Func<T, int?>> resultadoLocal,
            Expression<Func<T, int?>> resultadoVisitante)
        {
            var estadoFunc = estado.Compile();
            v.RuleFor(estado).Must(e => e == 'P' || e == 'E' || e == 'T')
                .WithMessage("El estado debe ser 'P' (previa), 'E' (en curso) o 'T' (terminado).");
            v.When(x => { var e = estadoFunc(x); return e == 'E' || e == 'T'; }, () =>
            {
                v.RuleFor(resultadoLocal).NotNull().GreaterThanOrEqualTo(0)
                    .WithMessage("Los goles del local son requeridos (>= 0) para un partido en curso o terminado.");
                v.RuleFor(resultadoVisitante).NotNull().GreaterThanOrEqualTo(0)
                    .WithMessage("Los goles del visitante son requeridos (>= 0) para un partido en curso o terminado.");
            });
        }
    }

    // Valida la coherencia de las relaciones del partido (existencia + pertenencia al torneo).
    public sealed class PartidoRelacionesValidator
    {
        private readonly IRepository<Torneo> _torneos;
        private readonly IRepository<Grupo> _grupos;
        private readonly IRepository<Equipo> _equipos;
        private readonly IRepository<TipoPartido> _tipos;
        private readonly IRepository<Fase> _fases;
        private readonly IRepository<Partido> _partidos;

        public PartidoRelacionesValidator(IRepository<Torneo> torneos, IRepository<Grupo> grupos,
            IRepository<Equipo> equipos, IRepository<TipoPartido> tipos, IRepository<Fase> fases, IRepository<Partido> partidos)
        { _torneos = torneos; _grupos = grupos; _equipos = equipos; _tipos = tipos; _fases = fases; _partidos = partidos; }

        public async Task<Error?> ValidarAsync(int torneoId, int? grupoId, int faseId, int? equipoLocalId, int? equipoVisitanteId,
            int tipoPartidoId, int? equipoGanadorId, int? partidoGanadorLocalId, int? partidoGanadorVisitanteId, bool porDefinirse, CancellationToken ct)
        {
            if (!await _torneos.GetDbSet().AsNoTracking().AnyAsync(t => t.Id == torneoId, ct))
                return PartidoAdminErrors.TorneoNotFound;
            if (!await _fases.GetDbSet().AsNoTracking().AnyAsync(f => f.Id == faseId, ct))
                return PartidoAdminErrors.FaseNotFound;
            if (!await _tipos.GetDbSet().AsNoTracking().AnyAsync(t => t.Id == tipoPartidoId, ct))
                return PartidoAdminErrors.TipoPartidoNotFound;

            if (porDefinirse)
            {
                // Un partido "por definirse" se arma del árbol y solo puede ser de eliminatoria.
                if (FasesConocidas.Tipo(faseId) != TipoFase.Eliminatoria)
                    return PartidoAdminErrors.FaseNoEliminatoria;
            }
            else
            {
                if (equipoLocalId == equipoVisitanteId) return PartidoAdminErrors.MismosEquipos;
                // El grupo es opcional: solo se valida si viene informado.
                if (grupoId is int g && !await _grupos.GetDbSet().AsNoTracking().AnyAsync(x => x.Id == g && x.TorneoId == torneoId, ct))
                    return PartidoAdminErrors.GrupoInvalido;
                if (equipoLocalId is not int el || !await _equipos.GetDbSet().AsNoTracking().AnyAsync(e => e.Id == el && e.TorneoId == torneoId, ct))
                    return PartidoAdminErrors.EquipoLocalInvalido;
                if (equipoVisitanteId is not int ev || !await _equipos.GetDbSet().AsNoTracking().AnyAsync(e => e.Id == ev && e.TorneoId == torneoId, ct))
                    return PartidoAdminErrors.EquipoVisitanteInvalido;
            }

            // Campos opcionales de eliminatoria: validar solo si vienen informados.
            if (equipoGanadorId.HasValue &&
                !await _equipos.GetDbSet().AsNoTracking().AnyAsync(e => e.Id == equipoGanadorId.Value && e.TorneoId == torneoId, ct))
                return PartidoAdminErrors.EquipoGanadorInvalido;
            if (partidoGanadorLocalId.HasValue &&
                !await _partidos.GetDbSet().AsNoTracking().AnyAsync(p => p.Id == partidoGanadorLocalId.Value, ct))
                return PartidoAdminErrors.PartidoGanadorInvalido;
            if (partidoGanadorVisitanteId.HasValue &&
                !await _partidos.GetDbSet().AsNoTracking().AnyAsync(p => p.Id == partidoGanadorVisitanteId.Value, ct))
                return PartidoAdminErrors.PartidoGanadorInvalido;

            return null;
        }
    }
}

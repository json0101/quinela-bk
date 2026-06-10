using Microsoft.EntityFrameworkCore;
using Quinela.Domain.Entities;

namespace Quinela.Infrastructure.Persistence;

/// <summary>
/// DbContext de Quinela. Las configuraciones de cada entidad viven en
/// Persistence/Configurations y se aplican automaticamente por assembly.
/// </summary>
public class QuinelaContext : DbContext
{
    public QuinelaContext(DbContextOptions<QuinelaContext> options) : base(options)
    {
    }

    public DbSet<Equipo> Equipos => Set<Equipo>();
    public DbSet<Grupo> Grupos => Set<Grupo>();
    public DbSet<GrupoEquipo> GruposEquipos => Set<GrupoEquipo>();
    public DbSet<TipoPartido> TiposPartido => Set<TipoPartido>();
    public DbSet<Partido> Partidos => Set<Partido>();
    public DbSet<Prediccion> Predicciones => Set<Prediccion>();
    public DbSet<Ranking> Rankings => Set<Ranking>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(QuinelaContext).Assembly);
    }
}

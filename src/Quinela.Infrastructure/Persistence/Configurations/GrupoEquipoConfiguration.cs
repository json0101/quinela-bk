using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quinela.Domain.Entities;

namespace Quinela.Infrastructure.Persistence.Configurations
{
    public class GrupoEquipoConfiguration : BaseEntityConfiguration<GrupoEquipo>
    {
        public override void ConfigureEntity(EntityTypeBuilder<GrupoEquipo> builder)
        {
            builder.ToTable("grupos_equipos");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.TorneoId).HasColumnName("torneo_id").IsRequired();
            builder.Property(x => x.GrupoId).HasColumnName("grupo_id").IsRequired();
            builder.Property(x => x.EquipoId).HasColumnName("equipo_id").IsRequired();
            builder.Property(x => x.Pts).HasColumnName("pts").IsRequired();
            builder.Property(x => x.GF).HasColumnName("gf").IsRequired();
            builder.Property(x => x.GC).HasColumnName("gc").IsRequired();
            builder.Property(x => x.Diff).HasColumnName("diff").IsRequired();
            builder.Property(x => x.Posicion).HasColumnName("posicion").IsRequired();

            builder.HasOne(x => x.Torneo)
                .WithMany()
                .HasForeignKey(x => x.TorneoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Grupo)
                .WithMany(g => g.GrupoEquipos)
                .HasForeignKey(x => x.GrupoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Equipo)
                .WithMany(e => e.GrupoEquipos)
                .HasForeignKey(x => x.EquipoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Un equipo no se repite dentro del mismo grupo.
            builder.HasIndex(x => new { x.GrupoId, x.EquipoId }).IsUnique();

            builder.HasData(
                new GrupoEquipo { Id = 1, TorneoId = 1, GrupoId = 1, EquipoId = 1, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 1, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 2, TorneoId = 1, GrupoId = 1, EquipoId = 2, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 2, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 3, TorneoId = 1, GrupoId = 1, EquipoId = 3, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 3, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 4, TorneoId = 1, GrupoId = 1, EquipoId = 4, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 4, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 5, TorneoId = 1, GrupoId = 2, EquipoId = 5, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 1, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 6, TorneoId = 1, GrupoId = 2, EquipoId = 6, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 2, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 7, TorneoId = 1, GrupoId = 2, EquipoId = 7, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 3, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 8, TorneoId = 1, GrupoId = 2, EquipoId = 8, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 4, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 9, TorneoId = 1, GrupoId = 3, EquipoId = 9, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 1, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 10, TorneoId = 1, GrupoId = 3, EquipoId = 10, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 2, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 11, TorneoId = 1, GrupoId = 3, EquipoId = 11, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 3, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 12, TorneoId = 1, GrupoId = 3, EquipoId = 12, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 4, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 13, TorneoId = 1, GrupoId = 4, EquipoId = 13, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 1, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 14, TorneoId = 1, GrupoId = 4, EquipoId = 14, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 2, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 15, TorneoId = 1, GrupoId = 4, EquipoId = 15, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 3, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 16, TorneoId = 1, GrupoId = 4, EquipoId = 16, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 4, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 17, TorneoId = 1, GrupoId = 5, EquipoId = 17, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 1, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 18, TorneoId = 1, GrupoId = 5, EquipoId = 18, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 2, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 19, TorneoId = 1, GrupoId = 5, EquipoId = 19, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 3, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 20, TorneoId = 1, GrupoId = 5, EquipoId = 20, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 4, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 21, TorneoId = 1, GrupoId = 6, EquipoId = 21, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 1, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 22, TorneoId = 1, GrupoId = 6, EquipoId = 22, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 2, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 23, TorneoId = 1, GrupoId = 6, EquipoId = 23, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 3, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 24, TorneoId = 1, GrupoId = 6, EquipoId = 24, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 4, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 25, TorneoId = 1, GrupoId = 7, EquipoId = 25, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 1, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 26, TorneoId = 1, GrupoId = 7, EquipoId = 26, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 2, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 27, TorneoId = 1, GrupoId = 7, EquipoId = 27, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 3, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 28, TorneoId = 1, GrupoId = 7, EquipoId = 28, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 4, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 29, TorneoId = 1, GrupoId = 8, EquipoId = 29, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 1, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 30, TorneoId = 1, GrupoId = 8, EquipoId = 30, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 2, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 31, TorneoId = 1, GrupoId = 8, EquipoId = 31, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 3, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 32, TorneoId = 1, GrupoId = 8, EquipoId = 32, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 4, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 33, TorneoId = 1, GrupoId = 9, EquipoId = 33, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 1, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 34, TorneoId = 1, GrupoId = 9, EquipoId = 34, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 2, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 35, TorneoId = 1, GrupoId = 9, EquipoId = 35, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 3, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 36, TorneoId = 1, GrupoId = 9, EquipoId = 36, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 4, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 37, TorneoId = 1, GrupoId = 10, EquipoId = 37, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 1, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 38, TorneoId = 1, GrupoId = 10, EquipoId = 38, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 2, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 39, TorneoId = 1, GrupoId = 10, EquipoId = 39, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 3, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 40, TorneoId = 1, GrupoId = 10, EquipoId = 40, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 4, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 41, TorneoId = 1, GrupoId = 11, EquipoId = 41, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 1, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 42, TorneoId = 1, GrupoId = 11, EquipoId = 42, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 2, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 43, TorneoId = 1, GrupoId = 11, EquipoId = 43, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 3, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 44, TorneoId = 1, GrupoId = 11, EquipoId = 44, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 4, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 45, TorneoId = 1, GrupoId = 12, EquipoId = 45, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 1, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 46, TorneoId = 1, GrupoId = 12, EquipoId = 46, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 2, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 47, TorneoId = 1, GrupoId = 12, EquipoId = 47, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 3, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new GrupoEquipo { Id = 48, TorneoId = 1, GrupoId = 12, EquipoId = 48, Pts = 0, GF = 0, GC = 0, Diff = 0, Posicion = 4, CreatedAt = Seed, CreatedBy = "seed", Active = true }
            );
        }
    }
}

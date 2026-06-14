using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quinela.Domain.Entities;

namespace Quinela.Infrastructure.Persistence.Configurations
{
    public class PartidoConfiguration : BaseEntityConfiguration<Partido>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Partido> builder)
        {
            builder.ToTable("partidos");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.FechaPartido).HasColumnName("fecha_partido").IsRequired();
            builder.Property(x => x.TorneoId).HasColumnName("torneo_id").IsRequired();
            builder.Property(x => x.GrupoId).HasColumnName("grupo_id").IsRequired();
            builder.Property(x => x.EquipoLocalId).HasColumnName("equipo_local_id").IsRequired();
            builder.Property(x => x.EquipoVisitanteId).HasColumnName("equipo_visitante_id").IsRequired();
            builder.Property(x => x.ResultadoLocalId).HasColumnName("resultado_local_id");
            builder.Property(x => x.ResultadoVisitanteId).HasColumnName("resultado_visitante_id");
            builder.Property(x => x.PtsLocal).HasColumnName("pts_local");
            builder.Property(x => x.PtsVisitante).HasColumnName("pts_visitante");
            builder.Property(x => x.TipoPartidoId).HasColumnName("tipo_partido_id").IsRequired();
            builder.Property(x => x.Estado).HasColumnName("estado").HasColumnType("char(1)").IsRequired();
            builder.Property(x => x.PartidoIdApi).HasColumnName("partido_id_api").HasMaxLength(40);

            builder.HasOne(x => x.Torneo)
                .WithMany()
                .HasForeignKey(x => x.TorneoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Grupo)
                .WithMany()
                .HasForeignKey(x => x.GrupoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.EquipoLocal)
                .WithMany()
                .HasForeignKey(x => x.EquipoLocalId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.EquipoVisitante)
                .WithMany()
                .HasForeignKey(x => x.EquipoVisitanteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.TipoPartido)
                .WithMany()
                .HasForeignKey(x => x.TipoPartidoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new Partido { Id = 1, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 11, 0, 0, 0, DateTimeKind.Utc), GrupoId = 1, EquipoLocalId = 1, EquipoVisitanteId = 2, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 2, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 11, 0, 0, 0, DateTimeKind.Utc), GrupoId = 1, EquipoLocalId = 3, EquipoVisitanteId = 4, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 3, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 12, 0, 0, 0, DateTimeKind.Utc), GrupoId = 2, EquipoLocalId = 5, EquipoVisitanteId = 6, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 4, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 12, 0, 0, 0, DateTimeKind.Utc), GrupoId = 4, EquipoLocalId = 13, EquipoVisitanteId = 14, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 5, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 13, 0, 0, 0, DateTimeKind.Utc), GrupoId = 2, EquipoLocalId = 7, EquipoVisitanteId = 8, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 6, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 13, 0, 0, 0, DateTimeKind.Utc), GrupoId = 3, EquipoLocalId = 9, EquipoVisitanteId = 10, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 7, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 13, 0, 0, 0, DateTimeKind.Utc), GrupoId = 3, EquipoLocalId = 11, EquipoVisitanteId = 12, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 8, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 13, 0, 0, 0, DateTimeKind.Utc), GrupoId = 4, EquipoLocalId = 15, EquipoVisitanteId = 16, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 9, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 14, 0, 0, 0, DateTimeKind.Utc), GrupoId = 5, EquipoLocalId = 17, EquipoVisitanteId = 18, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 10, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 14, 0, 0, 0, DateTimeKind.Utc), GrupoId = 6, EquipoLocalId = 21, EquipoVisitanteId = 22, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 11, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 14, 0, 0, 0, DateTimeKind.Utc), GrupoId = 5, EquipoLocalId = 19, EquipoVisitanteId = 20, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 12, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 14, 0, 0, 0, DateTimeKind.Utc), GrupoId = 6, EquipoLocalId = 23, EquipoVisitanteId = 24, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 13, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 15, 0, 0, 0, DateTimeKind.Utc), GrupoId = 8, EquipoLocalId = 29, EquipoVisitanteId = 30, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 14, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 15, 0, 0, 0, DateTimeKind.Utc), GrupoId = 7, EquipoLocalId = 25, EquipoVisitanteId = 26, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 15, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 15, 0, 0, 0, DateTimeKind.Utc), GrupoId = 8, EquipoLocalId = 31, EquipoVisitanteId = 32, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 16, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 15, 0, 0, 0, DateTimeKind.Utc), GrupoId = 7, EquipoLocalId = 27, EquipoVisitanteId = 28, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 17, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 16, 0, 0, 0, DateTimeKind.Utc), GrupoId = 9, EquipoLocalId = 33, EquipoVisitanteId = 34, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 18, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 16, 0, 0, 0, DateTimeKind.Utc), GrupoId = 9, EquipoLocalId = 35, EquipoVisitanteId = 36, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 19, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 16, 0, 0, 0, DateTimeKind.Utc), GrupoId = 10, EquipoLocalId = 37, EquipoVisitanteId = 38, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 20, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 16, 0, 0, 0, DateTimeKind.Utc), GrupoId = 10, EquipoLocalId = 39, EquipoVisitanteId = 40, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 21, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 17, 0, 0, 0, DateTimeKind.Utc), GrupoId = 11, EquipoLocalId = 41, EquipoVisitanteId = 42, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 22, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 17, 0, 0, 0, DateTimeKind.Utc), GrupoId = 12, EquipoLocalId = 45, EquipoVisitanteId = 46, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 23, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 17, 0, 0, 0, DateTimeKind.Utc), GrupoId = 12, EquipoLocalId = 47, EquipoVisitanteId = 48, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 24, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 17, 0, 0, 0, DateTimeKind.Utc), GrupoId = 11, EquipoLocalId = 43, EquipoVisitanteId = 44, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 25, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 18, 0, 0, 0, DateTimeKind.Utc), GrupoId = 1, EquipoLocalId = 4, EquipoVisitanteId = 2, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 26, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 18, 0, 0, 0, DateTimeKind.Utc), GrupoId = 2, EquipoLocalId = 8, EquipoVisitanteId = 6, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 27, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 18, 0, 0, 0, DateTimeKind.Utc), GrupoId = 2, EquipoLocalId = 5, EquipoVisitanteId = 7, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 28, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 18, 0, 0, 0, DateTimeKind.Utc), GrupoId = 1, EquipoLocalId = 1, EquipoVisitanteId = 3, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 29, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 19, 0, 0, 0, DateTimeKind.Utc), GrupoId = 4, EquipoLocalId = 13, EquipoVisitanteId = 15, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 30, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 19, 0, 0, 0, DateTimeKind.Utc), GrupoId = 3, EquipoLocalId = 12, EquipoVisitanteId = 10, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 31, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 19, 0, 0, 0, DateTimeKind.Utc), GrupoId = 3, EquipoLocalId = 9, EquipoVisitanteId = 11, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 32, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 19, 0, 0, 0, DateTimeKind.Utc), GrupoId = 4, EquipoLocalId = 16, EquipoVisitanteId = 14, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 33, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 20, 0, 0, 0, DateTimeKind.Utc), GrupoId = 6, EquipoLocalId = 21, EquipoVisitanteId = 23, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 34, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 20, 0, 0, 0, DateTimeKind.Utc), GrupoId = 5, EquipoLocalId = 17, EquipoVisitanteId = 19, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 35, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 20, 0, 0, 0, DateTimeKind.Utc), GrupoId = 5, EquipoLocalId = 20, EquipoVisitanteId = 18, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 36, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 20, 0, 0, 0, DateTimeKind.Utc), GrupoId = 6, EquipoLocalId = 24, EquipoVisitanteId = 22, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 37, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 21, 0, 0, 0, DateTimeKind.Utc), GrupoId = 8, EquipoLocalId = 29, EquipoVisitanteId = 31, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 38, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 21, 0, 0, 0, DateTimeKind.Utc), GrupoId = 7, EquipoLocalId = 25, EquipoVisitanteId = 27, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 39, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 21, 0, 0, 0, DateTimeKind.Utc), GrupoId = 8, EquipoLocalId = 32, EquipoVisitanteId = 30, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 40, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 21, 0, 0, 0, DateTimeKind.Utc), GrupoId = 7, EquipoLocalId = 28, EquipoVisitanteId = 26, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 41, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 22, 0, 0, 0, DateTimeKind.Utc), GrupoId = 10, EquipoLocalId = 37, EquipoVisitanteId = 39, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 42, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 22, 0, 0, 0, DateTimeKind.Utc), GrupoId = 9, EquipoLocalId = 33, EquipoVisitanteId = 35, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 43, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 22, 0, 0, 0, DateTimeKind.Utc), GrupoId = 9, EquipoLocalId = 36, EquipoVisitanteId = 34, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 44, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 22, 0, 0, 0, DateTimeKind.Utc), GrupoId = 10, EquipoLocalId = 40, EquipoVisitanteId = 38, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 45, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 23, 0, 0, 0, DateTimeKind.Utc), GrupoId = 11, EquipoLocalId = 41, EquipoVisitanteId = 43, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 46, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 23, 0, 0, 0, DateTimeKind.Utc), GrupoId = 12, EquipoLocalId = 45, EquipoVisitanteId = 47, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 47, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 23, 0, 0, 0, DateTimeKind.Utc), GrupoId = 12, EquipoLocalId = 48, EquipoVisitanteId = 46, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 48, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 23, 0, 0, 0, DateTimeKind.Utc), GrupoId = 11, EquipoLocalId = 44, EquipoVisitanteId = 42, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 49, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 24, 0, 0, 0, DateTimeKind.Utc), GrupoId = 2, EquipoLocalId = 8, EquipoVisitanteId = 5, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 50, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 24, 0, 0, 0, DateTimeKind.Utc), GrupoId = 2, EquipoLocalId = 6, EquipoVisitanteId = 7, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 51, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 24, 0, 0, 0, DateTimeKind.Utc), GrupoId = 3, EquipoLocalId = 10, EquipoVisitanteId = 11, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 52, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 24, 0, 0, 0, DateTimeKind.Utc), GrupoId = 3, EquipoLocalId = 12, EquipoVisitanteId = 9, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 53, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 24, 0, 0, 0, DateTimeKind.Utc), GrupoId = 1, EquipoLocalId = 2, EquipoVisitanteId = 3, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 54, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 24, 0, 0, 0, DateTimeKind.Utc), GrupoId = 1, EquipoLocalId = 4, EquipoVisitanteId = 1, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 55, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 25, 0, 0, 0, DateTimeKind.Utc), GrupoId = 5, EquipoLocalId = 18, EquipoVisitanteId = 19, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 56, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 25, 0, 0, 0, DateTimeKind.Utc), GrupoId = 5, EquipoLocalId = 20, EquipoVisitanteId = 17, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 57, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 25, 0, 0, 0, DateTimeKind.Utc), GrupoId = 6, EquipoLocalId = 24, EquipoVisitanteId = 21, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 58, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 25, 0, 0, 0, DateTimeKind.Utc), GrupoId = 6, EquipoLocalId = 22, EquipoVisitanteId = 23, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 59, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 25, 0, 0, 0, DateTimeKind.Utc), GrupoId = 4, EquipoLocalId = 16, EquipoVisitanteId = 13, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 60, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 25, 0, 0, 0, DateTimeKind.Utc), GrupoId = 4, EquipoLocalId = 14, EquipoVisitanteId = 15, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 61, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 26, 0, 0, 0, DateTimeKind.Utc), GrupoId = 9, EquipoLocalId = 36, EquipoVisitanteId = 33, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 62, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 26, 0, 0, 0, DateTimeKind.Utc), GrupoId = 9, EquipoLocalId = 34, EquipoVisitanteId = 35, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 63, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 26, 0, 0, 0, DateTimeKind.Utc), GrupoId = 8, EquipoLocalId = 30, EquipoVisitanteId = 31, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 64, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 26, 0, 0, 0, DateTimeKind.Utc), GrupoId = 8, EquipoLocalId = 32, EquipoVisitanteId = 29, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 65, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 26, 0, 0, 0, DateTimeKind.Utc), GrupoId = 7, EquipoLocalId = 28, EquipoVisitanteId = 25, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 66, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 26, 0, 0, 0, DateTimeKind.Utc), GrupoId = 7, EquipoLocalId = 26, EquipoVisitanteId = 27, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 67, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 27, 0, 0, 0, DateTimeKind.Utc), GrupoId = 12, EquipoLocalId = 48, EquipoVisitanteId = 45, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 68, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 27, 0, 0, 0, DateTimeKind.Utc), GrupoId = 12, EquipoLocalId = 46, EquipoVisitanteId = 47, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 69, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 27, 0, 0, 0, DateTimeKind.Utc), GrupoId = 11, EquipoLocalId = 44, EquipoVisitanteId = 41, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 70, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 27, 0, 0, 0, DateTimeKind.Utc), GrupoId = 11, EquipoLocalId = 42, EquipoVisitanteId = 43, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 71, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 27, 0, 0, 0, DateTimeKind.Utc), GrupoId = 10, EquipoLocalId = 38, EquipoVisitanteId = 39, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Partido { Id = 72, TorneoId = 1, FechaPartido = new DateTime(2026, 6, 27, 0, 0, 0, DateTimeKind.Utc), GrupoId = 10, EquipoLocalId = 40, EquipoVisitanteId = 37, TipoPartidoId = 1, Estado = 'P', CreatedAt = Seed, CreatedBy = "seed", Active = true }
            );
        }
    }
}

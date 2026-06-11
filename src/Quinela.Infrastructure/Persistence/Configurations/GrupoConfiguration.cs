using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quinela.Domain.Entities;

namespace Quinela.Infrastructure.Persistence.Configurations
{
    public class GrupoConfiguration : BaseEntityConfiguration<Grupo>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Grupo> builder)
        {
            builder.ToTable("grupos");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(5).IsRequired();
            builder.Property(x => x.TorneoId).HasColumnName("torneo_id").IsRequired();
            builder.HasOne(x => x.Torneo).WithMany().HasForeignKey(x => x.TorneoId).OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(x => new { x.TorneoId, x.Nombre }).IsUnique();

            builder.HasData(
                new Grupo { Id = 1, TorneoId = 1, Nombre = "A", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Grupo { Id = 2, TorneoId = 1, Nombre = "B", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Grupo { Id = 3, TorneoId = 1, Nombre = "C", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Grupo { Id = 4, TorneoId = 1, Nombre = "D", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Grupo { Id = 5, TorneoId = 1, Nombre = "E", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Grupo { Id = 6, TorneoId = 1, Nombre = "F", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Grupo { Id = 7, TorneoId = 1, Nombre = "G", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Grupo { Id = 8, TorneoId = 1, Nombre = "H", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Grupo { Id = 9, TorneoId = 1, Nombre = "I", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Grupo { Id = 10, TorneoId = 1, Nombre = "J", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Grupo { Id = 11, TorneoId = 1, Nombre = "K", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Grupo { Id = 12, TorneoId = 1, Nombre = "L", CreatedAt = Seed, CreatedBy = "seed", Active = true }
            );
        }
    }
}

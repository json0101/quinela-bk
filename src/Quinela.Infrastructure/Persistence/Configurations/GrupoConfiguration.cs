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
            builder.HasIndex(x => x.Nombre).IsUnique();

            builder.HasData(
                new Grupo { Id = 1, Nombre = "A", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Grupo { Id = 2, Nombre = "B", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Grupo { Id = 3, Nombre = "C", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Grupo { Id = 4, Nombre = "D", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Grupo { Id = 5, Nombre = "E", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Grupo { Id = 6, Nombre = "F", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Grupo { Id = 7, Nombre = "G", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Grupo { Id = 8, Nombre = "H", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Grupo { Id = 9, Nombre = "I", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Grupo { Id = 10, Nombre = "J", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Grupo { Id = 11, Nombre = "K", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Grupo { Id = 12, Nombre = "L", CreatedAt = Seed, CreatedBy = "seed", Active = true }
            );
        }
    }
}

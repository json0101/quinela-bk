using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quinela.Domain.Entities;

namespace Quinela.Infrastructure.Persistence.Configurations
{
    public class TorneoConfiguration : BaseEntityConfiguration<Torneo>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Torneo> builder)
        {
            builder.ToTable("torneos");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.Descripcion).HasColumnName("descripcion").HasMaxLength(200).IsRequired();

            builder.HasData(
                new Torneo { Id = 1, Descripcion = "Copa Mundial de la FIFA 2026", CreatedAt = Seed, CreatedBy = "seed", Active = true }
            );
        }
    }
}

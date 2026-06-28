using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quinela.Domain.Entities;

namespace Quinela.Infrastructure.Persistence.Configurations
{
    public class FaseConfiguration : BaseEntityConfiguration<Fase>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Fase> builder)
        {
            builder.ToTable("fases");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.Descripcion).HasColumnName("descripcion").HasMaxLength(120).IsRequired();
            builder.Property(x => x.TorneoId).HasColumnName("torneo_id").IsRequired();

            builder.HasOne(x => x.Torneo)
                .WithMany()
                .HasForeignKey(x => x.TorneoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.TorneoId, x.Descripcion }).IsUnique();

            builder.HasData(
                new Fase { Id = 1, Descripcion = "Grupos", TorneoId = 1, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Fase { Id = 2, Descripcion = "Eliminatoria", TorneoId = 1, CreatedAt = Seed, CreatedBy = "seed", Active = true }
            );
        }
    }
}

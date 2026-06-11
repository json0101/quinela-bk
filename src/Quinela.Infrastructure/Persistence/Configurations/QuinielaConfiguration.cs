using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quinela.Domain.Entities;

namespace Quinela.Infrastructure.Persistence.Configurations
{
    public class QuinielaConfiguration : BaseEntityConfiguration<Quiniela>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Quiniela> builder)
        {
            builder.ToTable("quinielas");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(150).IsRequired();
            builder.Property(x => x.Reglas).HasColumnName("reglas").HasColumnType("text").IsRequired();
            builder.Property(x => x.TorneoId).HasColumnName("torneo_id").IsRequired();

            builder.HasOne(x => x.Torneo)
                .WithMany()
                .HasForeignKey(x => x.TorneoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new Quiniela { Id = 1, Nombre = "Quiniela Cattrachas", Reglas = "Reglas por definir.", TorneoId = 1, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Quiniela { Id = 2, Nombre = "Quiniela Tegra", Reglas = "Reglas por definir.", TorneoId = 1, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Quiniela { Id = 3, Nombre = "Quiniela Impex", Reglas = "Reglas por definir.", TorneoId = 1, CreatedAt = Seed, CreatedBy = "seed", Active = true }
            );
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quinela.Domain.Entities;

namespace Quinela.Infrastructure.Persistence.Configurations
{
    public class TipoPartidoConfiguration : BaseEntityConfiguration<TipoPartido>
    {
        public override void ConfigureEntity(EntityTypeBuilder<TipoPartido> builder)
        {
            builder.ToTable("tipos_partido");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.Descripcion).HasColumnName("descripcion").HasMaxLength(120).IsRequired();
            builder.Property(x => x.FaseId).HasColumnName("fase_id").IsRequired();
            builder.Property(x => x.PtsPartidoVictoria).HasColumnName("pts_partido_victoria").IsRequired();
            builder.Property(x => x.PtsPartidoEmpate).HasColumnName("pts_partido_empate").IsRequired();
            builder.Property(x => x.PtsQuinelaResultadoExacto).HasColumnName("pts_quinela_resultado_exacto").IsRequired();
            builder.Property(x => x.PtsQuinelaResultadoAcertado).HasColumnName("pts_quinela_resultado_acertado").IsRequired();
            builder.HasIndex(x => x.Descripcion).IsUnique();

            builder.HasOne(x => x.Fase)
                .WithMany()
                .HasForeignKey(x => x.FaseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new TipoPartido { Id = 1, Descripcion = "Fase de grupos", FaseId = 1, PtsPartidoVictoria = 0, PtsPartidoEmpate = 0, PtsQuinelaResultadoExacto = 0, PtsQuinelaResultadoAcertado = 0, CreatedAt = Seed, CreatedBy = "seed", Active = true }
            );
        }
    }
}

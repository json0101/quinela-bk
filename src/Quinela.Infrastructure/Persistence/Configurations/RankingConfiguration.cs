using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quinela.Domain.Entities;

namespace Quinela.Infrastructure.Persistence.Configurations
{
    public class RankingConfiguration : BaseEntityConfiguration<Ranking>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Ranking> builder)
        {
            builder.ToTable("ranking");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.QuinielaId).HasColumnName("quiniela_id").IsRequired();
            // Usuario de UserApp como texto: sin FK.
            builder.Property(x => x.Usuario).HasColumnName("usuario").HasMaxLength(250).IsRequired();
            builder.Property(x => x.Pts).HasColumnName("pts").IsRequired();
            builder.Property(x => x.ResultadoAtinado).HasColumnName("resultado_atinado").IsRequired();
            builder.Property(x => x.ResultadoExacto).HasColumnName("resultado_exacto").IsRequired();

            builder.HasOne(x => x.Quiniela)
                .WithMany()
                .HasForeignKey(x => x.QuinielaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Una sola fila de ranking por usuario y quiniela.
            builder.HasIndex(x => new { x.QuinielaId, x.Usuario }).IsUnique();
        }
    }
}

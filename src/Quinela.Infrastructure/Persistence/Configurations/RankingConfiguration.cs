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
            // Usuario de UserApp como texto: sin FK.
            builder.Property(x => x.Usuario).HasColumnName("usuario").HasMaxLength(250).IsRequired();
            builder.Property(x => x.Pts).HasColumnName("pts").IsRequired();
            builder.Property(x => x.ResultadoAtinado).HasColumnName("resultado_atinado").IsRequired();
            builder.Property(x => x.ResultadoExacto).HasColumnName("resultado_exacto").IsRequired();
        }
    }
}

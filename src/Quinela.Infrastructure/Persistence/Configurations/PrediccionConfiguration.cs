using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quinela.Domain.Entities;

namespace Quinela.Infrastructure.Persistence.Configurations
{
    public class PrediccionConfiguration : BaseEntityConfiguration<Prediccion>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Prediccion> builder)
        {
            builder.ToTable("predicciones");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.QuinielaId).HasColumnName("quiniela_id").IsRequired();
            builder.Property(x => x.PartidoId).HasColumnName("partido_id").IsRequired();
            // Nullable: se puede guardar solo el resultado del local y dejar el del visitante en null.
            builder.Property(x => x.Team1Resultado).HasColumnName("team_1_resultado");
            builder.Property(x => x.Team2Resultado).HasColumnName("team_2_resultado");
            // Usuario de UserApp como texto: sin FK, validado manualmente en la capa de aplicación.
            builder.Property(x => x.Username).HasColumnName("username").HasMaxLength(250).IsRequired();

            builder.HasOne(x => x.Quiniela)
                .WithMany()
                .HasForeignKey(x => x.QuinielaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Partido)
                .WithMany()
                .HasForeignKey(x => x.PartidoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Una sola predicción ACTIVA por usuario, quiniela y partido. Índice único parcial:
            // solo aplica a las activas, así las inactivas (soft delete) no chocan al recrear.
            builder.HasIndex(x => new { x.QuinielaId, x.PartidoId, x.Username })
                .IsUnique()
                .HasFilter("active = true");
        }
    }
}

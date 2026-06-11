using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quinela.Domain.Entities;

namespace Quinela.Infrastructure.Persistence.Configurations
{
    public class UsuarioQuinielaConfiguration : BaseEntityConfiguration<UsuarioQuiniela>
    {
        public override void ConfigureEntity(EntityTypeBuilder<UsuarioQuiniela> builder)
        {
            builder.ToTable("usuarios_quinielas");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            // user_id de sec.users (otra base de datos): solo un entero, sin FK.
            builder.Property(x => x.UserId).HasColumnName("user_id").IsRequired();
            builder.Property(x => x.QuinielaId).HasColumnName("quiniela_id").IsRequired();

            builder.HasOne(x => x.Quiniela)
                .WithMany()
                .HasForeignKey(x => x.QuinielaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Un usuario no puede tener el mismo acceso dos veces.
            builder.HasIndex(x => new { x.UserId, x.QuinielaId }).IsUnique();

            // Seed por defecto: Jason (user 1) a las 3 quinielas; Elmer (user 2) solo a Tegra (quiniela 2).
            builder.HasData(
                new UsuarioQuiniela { Id = 1, UserId = 1, QuinielaId = 1, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new UsuarioQuiniela { Id = 2, UserId = 1, QuinielaId = 2, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new UsuarioQuiniela { Id = 3, UserId = 1, QuinielaId = 3, CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new UsuarioQuiniela { Id = 4, UserId = 2, QuinielaId = 2, CreatedAt = Seed, CreatedBy = "seed", Active = true }
            );
        }
    }
}

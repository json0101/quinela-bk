using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quinela.Domain.Entities.Commons;

namespace Quinela.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuracion base reutilizable: mapea las columnas de auditoria (snake_case),
    /// igual que en Nemesis. Cada entidad concreta hereda y agrega lo suyo en ConfigureEntity.
    /// </summary>
    public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseEntity
    {
        // Fecha fija para el seed (HasData exige valores deterministas; UTC por Npgsql).
        protected static readonly DateTime Seed = new DateTime(2026, 6, 8, 0, 0, 0, DateTimeKind.Utc);

        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
            builder.Property(x => x.CreatedBy).HasMaxLength(250).HasColumnName("created_by").IsRequired();
            builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            builder.Property(x => x.UpdatedBy).HasMaxLength(250).HasColumnName("updated_by");
            builder.Property(x => x.Active).HasColumnName("active").IsRequired();

            ConfigureEntity(builder);
        }

        public abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);
    }
}

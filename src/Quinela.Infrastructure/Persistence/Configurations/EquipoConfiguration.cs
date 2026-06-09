using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quinela.Domain.Entities;

namespace Quinela.Infrastructure.Persistence.Configurations
{
    public class EquipoConfiguration : BaseEntityConfiguration<Equipo>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Equipo> builder)
        {
            builder.ToTable("equipos");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(120).IsRequired();
            builder.Property(x => x.Confederacion).HasColumnName("confederacion").HasMaxLength(20).IsRequired();
            builder.Property(x => x.Anfitrion).HasColumnName("anfitrion").IsRequired();
            builder.Property(x => x.UrlBandera).HasColumnName("url_bandera").HasMaxLength(60);
            builder.HasIndex(x => x.Nombre).IsUnique();

            builder.HasData(
                new Equipo { Id = 1, Nombre = "México", Confederacion = "CONCACAF", Anfitrion = true, UrlBandera = "mx.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 2, Nombre = "Sudáfrica", Confederacion = "CAF", Anfitrion = false, UrlBandera = "za.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 3, Nombre = "Corea del Sur", Confederacion = "AFC", Anfitrion = false, UrlBandera = "kr.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 4, Nombre = "República Checa", Confederacion = "UEFA", Anfitrion = false, UrlBandera = "cz.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 5, Nombre = "Canadá", Confederacion = "CONCACAF", Anfitrion = true, UrlBandera = "ca.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 6, Nombre = "Bosnia y Herzegovina", Confederacion = "UEFA", Anfitrion = false, UrlBandera = "ba.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 7, Nombre = "Qatar", Confederacion = "AFC", Anfitrion = false, UrlBandera = "qa.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 8, Nombre = "Suiza", Confederacion = "UEFA", Anfitrion = false, UrlBandera = "ch.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 9, Nombre = "Brasil", Confederacion = "CONMEBOL", Anfitrion = false, UrlBandera = "br.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 10, Nombre = "Marruecos", Confederacion = "CAF", Anfitrion = false, UrlBandera = "ma.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 11, Nombre = "Haití", Confederacion = "CONCACAF", Anfitrion = false, UrlBandera = "ht.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 12, Nombre = "Escocia", Confederacion = "UEFA", Anfitrion = false, UrlBandera = "gb-sct.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 13, Nombre = "Estados Unidos", Confederacion = "CONCACAF", Anfitrion = true, UrlBandera = "us.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 14, Nombre = "Paraguay", Confederacion = "CONMEBOL", Anfitrion = false, UrlBandera = "py.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 15, Nombre = "Australia", Confederacion = "AFC", Anfitrion = false, UrlBandera = "au.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 16, Nombre = "Turquía", Confederacion = "UEFA", Anfitrion = false, UrlBandera = "tr.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 17, Nombre = "Alemania", Confederacion = "UEFA", Anfitrion = false, UrlBandera = "de.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 18, Nombre = "Curazao", Confederacion = "CONCACAF", Anfitrion = false, UrlBandera = "cw.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 19, Nombre = "Costa de Marfil", Confederacion = "CAF", Anfitrion = false, UrlBandera = "ci.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 20, Nombre = "Ecuador", Confederacion = "CONMEBOL", Anfitrion = false, UrlBandera = "ec.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 21, Nombre = "Países Bajos", Confederacion = "UEFA", Anfitrion = false, UrlBandera = "nl.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 22, Nombre = "Japón", Confederacion = "AFC", Anfitrion = false, UrlBandera = "jp.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 23, Nombre = "Suecia", Confederacion = "UEFA", Anfitrion = false, UrlBandera = "se.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 24, Nombre = "Túnez", Confederacion = "CAF", Anfitrion = false, UrlBandera = "tn.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 25, Nombre = "Bélgica", Confederacion = "UEFA", Anfitrion = false, UrlBandera = "be.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 26, Nombre = "Egipto", Confederacion = "CAF", Anfitrion = false, UrlBandera = "eg.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 27, Nombre = "Irán", Confederacion = "AFC", Anfitrion = false, UrlBandera = "ir.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 28, Nombre = "Nueva Zelanda", Confederacion = "OFC", Anfitrion = false, UrlBandera = "nz.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 29, Nombre = "España", Confederacion = "UEFA", Anfitrion = false, UrlBandera = "es.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 30, Nombre = "Cabo Verde", Confederacion = "CAF", Anfitrion = false, UrlBandera = "cv.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 31, Nombre = "Arabia Saudita", Confederacion = "AFC", Anfitrion = false, UrlBandera = "sa.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 32, Nombre = "Uruguay", Confederacion = "CONMEBOL", Anfitrion = false, UrlBandera = "uy.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 33, Nombre = "Francia", Confederacion = "UEFA", Anfitrion = false, UrlBandera = "fr.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 34, Nombre = "Senegal", Confederacion = "CAF", Anfitrion = false, UrlBandera = "sn.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 35, Nombre = "Irak", Confederacion = "AFC", Anfitrion = false, UrlBandera = "iq.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 36, Nombre = "Noruega", Confederacion = "UEFA", Anfitrion = false, UrlBandera = "no.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 37, Nombre = "Argentina", Confederacion = "CONMEBOL", Anfitrion = false, UrlBandera = "ar.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 38, Nombre = "Argelia", Confederacion = "CAF", Anfitrion = false, UrlBandera = "dz.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 39, Nombre = "Austria", Confederacion = "UEFA", Anfitrion = false, UrlBandera = "at.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 40, Nombre = "Jordania", Confederacion = "AFC", Anfitrion = false, UrlBandera = "jo.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 41, Nombre = "Portugal", Confederacion = "UEFA", Anfitrion = false, UrlBandera = "pt.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 42, Nombre = "RD Congo", Confederacion = "CAF", Anfitrion = false, UrlBandera = "cd.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 43, Nombre = "Uzbekistán", Confederacion = "AFC", Anfitrion = false, UrlBandera = "uz.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 44, Nombre = "Colombia", Confederacion = "CONMEBOL", Anfitrion = false, UrlBandera = "co.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 45, Nombre = "Inglaterra", Confederacion = "UEFA", Anfitrion = false, UrlBandera = "gb-eng.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 46, Nombre = "Croacia", Confederacion = "UEFA", Anfitrion = false, UrlBandera = "hr.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 47, Nombre = "Ghana", Confederacion = "CAF", Anfitrion = false, UrlBandera = "gh.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true },
                new Equipo { Id = 48, Nombre = "Panamá", Confederacion = "CONCACAF", Anfitrion = false, UrlBandera = "pa.svg", CreatedAt = Seed, CreatedBy = "seed", Active = true }
            );
        }
    }
}

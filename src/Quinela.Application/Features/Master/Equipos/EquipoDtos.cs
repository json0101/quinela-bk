namespace Quinela.Application.Features.Master.Equipos
{
    public class EquipoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Confederacion { get; set; } = string.Empty;
        public bool Anfitrion { get; set; }
        public string? UrlBandera { get; set; }
        public string? EquipoIdApi { get; set; }
        public string? EquipoIdApiLargo { get; set; }
        public int TorneoId { get; set; }
        public string Torneo { get; set; } = string.Empty;
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class EquipoCreateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Confederacion { get; set; } = string.Empty;
        public bool Anfitrion { get; set; }
        public string? UrlBandera { get; set; }
        public string? EquipoIdApi { get; set; }
        public string? EquipoIdApiLargo { get; set; }
        public int TorneoId { get; set; }
        public bool Active { get; set; } = true;
    }

    public class EquipoUpdateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Confederacion { get; set; } = string.Empty;
        public bool Anfitrion { get; set; }
        public string? UrlBandera { get; set; }
        public string? EquipoIdApi { get; set; }
        public string? EquipoIdApiLargo { get; set; }
        public int TorneoId { get; set; }
        public bool Active { get; set; }
    }
}

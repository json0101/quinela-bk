namespace Quinela.Application.Features.Master.Grupos
{
    public class GrupoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int TorneoId { get; set; }
        public string Torneo { get; set; } = string.Empty;
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class GrupoCreateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public int TorneoId { get; set; }
        public bool Active { get; set; } = true;
    }

    public class GrupoUpdateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public int TorneoId { get; set; }
        public bool Active { get; set; }
    }
}

namespace Quinela.Application.Features.Master.Quinielas
{
    public class QuinielaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Reglas { get; set; } = string.Empty;
        public int TorneoId { get; set; }
        public string Torneo { get; set; } = string.Empty;
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class QuinielaCreateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Reglas { get; set; } = string.Empty;
        public int TorneoId { get; set; }
        public bool Active { get; set; } = true;
    }

    public class QuinielaUpdateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Reglas { get; set; } = string.Empty;
        public int TorneoId { get; set; }
        public bool Active { get; set; }
    }
}

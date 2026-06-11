namespace Quinela.Application.Features.Predicciones
{
    public class PrediccionDto
    {
        public int Id { get; set; }
        public int QuinielaId { get; set; }
        public int PartidoId { get; set; }
        public int? Team1Resultado { get; set; }
        public int? Team2Resultado { get; set; }
        public string Username { get; set; } = string.Empty;
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class PrediccionCreateDto
    {
        public int QuinielaId { get; set; }
        public int PartidoId { get; set; }
        public int? Team1Resultado { get; set; }
        public int? Team2Resultado { get; set; }
        public bool Active { get; set; } = true;
    }

    public class PrediccionUpdateDto
    {
        public int QuinielaId { get; set; }
        public int PartidoId { get; set; }
        public int? Team1Resultado { get; set; }
        public int? Team2Resultado { get; set; }
        public bool Active { get; set; }
    }

    public class PrediccionUpsertDto
    {
        public int QuinielaId { get; set; }
        public int PartidoId { get; set; }
        public int? Team1Resultado { get; set; }
        public int? Team2Resultado { get; set; }
    }
}

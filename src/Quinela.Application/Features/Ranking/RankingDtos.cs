namespace Quinela.Application.Features.Ranking
{
    public class RankingDto
    {
        public int Id { get; set; }
        // Posición en la tabla (1 = primero). Empatados comparten posición y la
        // siguiente se "salta" (ranking de competición estándar): 1,1,3...
        public int Posicion { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public int Pts { get; set; }
        public int ResultadoAtinado { get; set; }
        public int ResultadoExacto { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}

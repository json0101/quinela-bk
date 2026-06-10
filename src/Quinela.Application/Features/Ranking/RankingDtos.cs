namespace Quinela.Application.Features.Ranking
{
    public class RankingDto
    {
        public int Id { get; set; }
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

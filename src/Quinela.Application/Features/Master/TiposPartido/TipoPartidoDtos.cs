namespace Quinela.Application.Features.Master.TiposPartido
{
    public class TipoPartidoDto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int PtsPartidoVictoria { get; set; }
        public int PtsPartidoEmpate { get; set; }
        public int PtsQuinelaResultadoExacto { get; set; }
        public int PtsQuinelaResultadoAcertado { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class TipoPartidoCreateDto
    {
        public string Descripcion { get; set; } = string.Empty;
        public int PtsPartidoVictoria { get; set; }
        public int PtsPartidoEmpate { get; set; }
        public int PtsQuinelaResultadoExacto { get; set; }
        public int PtsQuinelaResultadoAcertado { get; set; }
        public bool Active { get; set; } = true;
    }

    public class TipoPartidoUpdateDto
    {
        public string Descripcion { get; set; } = string.Empty;
        public int PtsPartidoVictoria { get; set; }
        public int PtsPartidoEmpate { get; set; }
        public int PtsQuinelaResultadoExacto { get; set; }
        public int PtsQuinelaResultadoAcertado { get; set; }
        public bool Active { get; set; }
    }
}

namespace Quinela.Application.Features.Grupos
{
    /// <summary>Un grupo con su tabla de posiciones (para la vista de tarjetas).</summary>
    public class GrupoTablaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public List<GrupoEquipoTablaDto> Equipos { get; set; } = new();
    }

    public class GrupoEquipoTablaDto
    {
        public int EquipoId { get; set; }
        public string Equipo { get; set; } = string.Empty;
        public string? UrlBandera { get; set; }
        public int Posicion { get; set; }
        public int Pts { get; set; }
        public int GF { get; set; }
        public int GC { get; set; }
        public int Diff { get; set; }
    }
}

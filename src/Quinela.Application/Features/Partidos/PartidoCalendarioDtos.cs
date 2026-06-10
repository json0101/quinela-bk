namespace Quinela.Application.Features.Partidos
{
    /// <summary>Un partido para la vista de calendario (tarjetas).</summary>
    public class PartidoCalendarioDto
    {
        public int Id { get; set; }
        public DateTime FechaPartido { get; set; }
        public string Grupo { get; set; } = string.Empty;
        public char Estado { get; set; }
        public EquipoCalendarioDto Local { get; set; } = new();
        public EquipoCalendarioDto Visitante { get; set; } = new();

        // Resultado real del partido (goles). null mientras no se ha jugado.
        public int? ResultadoLocal { get; set; }
        public int? ResultadoVisitante { get; set; }

        // Predicción del usuario autenticado para este partido (null si no tiene).
        public PrediccionResumenDto? Prediccion { get; set; }

        // Puntos de quiniela ganados con la predicción (null si el partido aún no tiene resultado).
        public int? PuntosGanados { get; set; }
        // "exacto" | "acertado" | "ninguno" | null
        public string? CategoriaPuntos { get; set; }
    }

    public class EquipoCalendarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? UrlBandera { get; set; }
    }

    public class PrediccionResumenDto
    {
        public int Id { get; set; }
        public int? Team1Resultado { get; set; }
        public int? Team2Resultado { get; set; }
    }
}

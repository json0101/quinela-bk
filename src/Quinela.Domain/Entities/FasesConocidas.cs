namespace Quinela.Domain.Entities
{
    /// <summary>
    /// Tipo de fase para decidir cómo se arma un partido. "Grupos" es la fase
    /// sembrada por defecto; cualquier otra fase se trata como "Eliminatoria"
    /// (octavos, cuartos, final, etc. comparten la misma lógica de definición).
    /// </summary>
    public enum TipoFase
    {
        Grupos = 1,
        Eliminatoria = 2,
    }

    /// <summary>Resuelve el <see cref="TipoFase"/> a partir del id de la fase.</summary>
    public static class FasesConocidas
    {
        public static TipoFase Tipo(int faseId) =>
            faseId == Fase.GruposId ? TipoFase.Grupos : TipoFase.Eliminatoria;
    }
}

namespace Quinela.Application.Common.Abstractions
{
    /// <summary>
    /// Recalcula la tabla de posiciones de los grupos y el ranking de la quiniela
    /// a partir de los partidos jugados (estado 'E' o 'T') y las predicciones.
    /// Es idempotente: parte de cero y vuelve a sumar, así puede correrse en cada
    /// cambio de estado sin duplicar puntos.
    /// </summary>
    public interface IRankingService
    {
        // Recalcula las posiciones de los grupos del torneo y el ranking de TODAS sus quinielas.
        Task RecalcularAsync(int torneoId, CancellationToken ct = default);
    }
}

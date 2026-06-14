using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Quinela.Application.Features.AutomationMatch
{
    // Datos que nos interesan de un game del API worldcup26.ir.
    public sealed record GameApi(int HomeScore, int AwayScore, bool Finished);

    public interface IWorldCupApiClient
    {
        // GET /get/game/{idLargo}. Devuelve null si no existe o falla la llamada.
        Task<GameApi?> GetGameAsync(string idLargo, CancellationToken ct);
    }

    // Cliente tipado (HttpClient con BaseAddress = AppSetting:WorldCupApiBaseUrl).
    // Los valores numéricos del API llegan como string ("0", "TRUE"), por eso se parsean.
    public sealed class WorldCupApiClient : IWorldCupApiClient
    {
        private readonly HttpClient _http;
        public WorldCupApiClient(HttpClient http) => _http = http;

        public async Task<GameApi?> GetGameAsync(string idLargo, CancellationToken ct)
        {
            var resp = await _http.GetAsync($"/get/game/{idLargo}", ct);
            if (!resp.IsSuccessStatusCode) return null;

            var body = await resp.Content.ReadFromJsonAsync<GameResponse>(cancellationToken: ct);
            var g = body?.Game;
            if (g is null) return null;

            return new GameApi(ParseInt(g.HomeScore), ParseInt(g.AwayScore), EsTrue(g.Finished));
        }

        private static int ParseInt(string? s) => int.TryParse(s, out var v) ? v : 0;
        private static bool EsTrue(string? s) => string.Equals(s?.Trim(), "TRUE", StringComparison.OrdinalIgnoreCase);

        private sealed class GameResponse
        {
            [JsonPropertyName("game")] public GameRaw? Game { get; set; }
        }

        private sealed class GameRaw
        {
            [JsonPropertyName("home_score")] public string? HomeScore { get; set; }
            [JsonPropertyName("away_score")] public string? AwayScore { get; set; }
            [JsonPropertyName("finished")] public string? Finished { get; set; }
        }
    }
}

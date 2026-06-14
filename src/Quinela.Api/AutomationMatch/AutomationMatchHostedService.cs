using Microsoft.Extensions.Options;
using Quinela.Application.Commons;
using Quinela.Application.Features.AutomationMatch;

namespace Quinela.Api.AutomationMatch
{
    /// <summary>
    /// Orquesta la automatización de partidos cada 30 segundos:
    ///  1) MatchStartVerificationService: arranca (E, 0-0) los partidos cuya hora ya llegó.
    ///  2) MatchStatusVerificationService: consulta el API y actualiza goles/estado de los 'E'.
    /// Cada paso corre en su propio scope (DbContext) para mantener limpia la unidad de trabajo.
    /// Se apaga con AppSetting:AutomationMatchEnabled = false.
    /// </summary>
    public sealed class AutomationMatchHostedService : BackgroundService
    {
        private static readonly TimeSpan Intervalo = TimeSpan.FromSeconds(30);

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IOptions<AppSetting> _options;
        private readonly ILogger<AutomationMatchHostedService> _logger;

        public AutomationMatchHostedService(IServiceScopeFactory scopeFactory,
            IOptions<AppSetting> options, ILogger<AutomationMatchHostedService> logger)
        {
            _scopeFactory = scopeFactory;
            _options = options;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!_options.Value.AutomationMatchEnabled)
            {
                _logger.LogInformation("AutomationMatch deshabilitado (AppSetting:AutomationMatchEnabled = false).");
                return;
            }

            _logger.LogInformation("AutomationMatch activo: cada {Seg}s.", Intervalo.TotalSeconds);
            using var timer = new PeriodicTimer(Intervalo);
            do
            {
                try
                {
                    await EjecutarTickAsync(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error en el ciclo de AutomationMatch.");
                }
            }
            while (await EsperarSiguienteAsync(timer, stoppingToken));
        }

        private static async Task<bool> EsperarSiguienteAsync(PeriodicTimer timer, CancellationToken ct)
        {
            try { return await timer.WaitForNextTickAsync(ct); }
            catch (OperationCanceledException) { return false; }
        }

        private async Task EjecutarTickAsync(CancellationToken ct)
        {
            // Paso 1: arrancar los partidos que ya empezaron.
            using (var scope = _scopeFactory.CreateScope())
                await scope.ServiceProvider.GetRequiredService<MatchStartVerificationService>().EjecutarAsync(ct);

            // Paso 2: sincronizar goles/estado de los partidos en curso.
            using (var scope = _scopeFactory.CreateScope())
                await scope.ServiceProvider.GetRequiredService<MatchStatusVerificationService>().EjecutarAsync(ct);
        }
    }
}

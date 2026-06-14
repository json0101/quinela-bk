namespace Quinela.Application.Commons
{
    public class AppSetting
    {
        public string? JwtSecret { get; set; }
        public string? JwtIssuer { get; set; }
        public string? JwtAudience { get; set; }
        public string? AutoMapperLicence { get; set; }

        // Automatización de partidos (worldcup26.ir).
        public string? WorldCupApiBaseUrl { get; set; }
        public bool AutomationMatchEnabled { get; set; }
    }
}

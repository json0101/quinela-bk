using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Quinela.Infrastructure.Persistence
{
    /// <summary>
    /// Factory de tiempo de diseño: la usa la CLI de EF (dotnet ef migrations/database)
    /// para crear el contexto sin levantar toda la API. Lee la cadena "Quinela"
    /// desde el appsettings de Quinela.Api.
    /// </summary>
    public class QuinelaContextFactory : IDesignTimeDbContextFactory<QuinelaContext>
    {
        public QuinelaContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
                ?? "Development";

            var basePath = Path.GetFullPath(
                Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Quinela.Api")
            );

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("Quinela");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "La cadena de conexión 'Quinela' no está definida en appsettings.json.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<QuinelaContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new QuinelaContext(optionsBuilder.Options);
        }
    }
}

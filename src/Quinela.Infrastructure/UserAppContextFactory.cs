using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using UserApp.Domain;

namespace Quinela.Infrastructure
{
    /// <summary>
    /// Factory de diseño para UserAppContext. Quinela toma control del esquema 'sec'
    /// (usuarios/roles/screens/applications) vía migraciones EF hospedadas en este
    /// proyecto (Quinela.Infrastructure). Lee la cadena "UserApp" del appsettings de la API.
    /// </summary>
    public class UserAppContextFactory : IDesignTimeDbContextFactory<UserAppContext>
    {
        public UserAppContext CreateDbContext(string[] args)
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

            var connectionString = configuration.GetConnectionString("UserApp");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("La cadena de conexión 'UserApp' no está definida.");

            var optionsBuilder = new DbContextOptionsBuilder<UserAppContext>();
            optionsBuilder.UseNpgsql(connectionString, b => b.MigrationsAssembly("Quinela.Infrastructure"));

            return new UserAppContext(optionsBuilder.Options);
        }
    }
}

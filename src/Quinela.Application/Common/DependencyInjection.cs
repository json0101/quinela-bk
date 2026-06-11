using System.Reflection;
using Quinela.Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Quinela.Application.Common
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
            services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            // Validador de coherencia de relaciones del partido (clase normal, no se auto-registra).
            services.AddScoped<Features.Master.Partidos.PartidoRelacionesValidator>();

            // Lector de usuarios de UserApp (query plano sobre UserAppContext).
            services.AddScoped<Features.Master.UsuariosQuinielas.UsuariosAppReader>();

            return services;
        }
    }
}

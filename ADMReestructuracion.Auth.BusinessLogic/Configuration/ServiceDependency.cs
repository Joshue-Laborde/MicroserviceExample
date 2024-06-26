using ADMReestructuracion.Auth.BusinessLogic.Service;
using Microsoft.Extensions.DependencyInjection;

namespace ADMReestructuracion.Auth.BusinessLogic.Configuration
{
    public static class ServiceDependency
    {
        public static void ConfigureAppService(this IServiceCollection services)
        {
            services.AddScoped<IUsuarioService, UsuarioService>();

        }
    }
}

using ADMReestructuracion.Auth.BusinessLogic.Service;
using ADMReestructuracion.Auth.Domain.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

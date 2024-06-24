using ADMReestructuracion.Auth.BusinessLogic.Service;
using ADMReestructuracion.Auth.DataAccess.Models;
using ADMReestructuracion.Auth.Domain.Interface;
using ADMReestructuracion.Common.Data.Repositories;
using ADMReestructuracion.Common.Interfaces;
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

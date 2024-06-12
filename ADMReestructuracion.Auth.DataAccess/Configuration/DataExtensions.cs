using ADMReestructuracion.Auth.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADMReestructuracion.Auth.DataAccess.Configuration
{
    public static class DataExtensions
    {
        public static void ConfigureDataService(this IServiceCollection services)
        {
            services.AddScoped<DbContext, AuthContext>();
            services.AddScoped<AuthContext>();
        }
    }
}

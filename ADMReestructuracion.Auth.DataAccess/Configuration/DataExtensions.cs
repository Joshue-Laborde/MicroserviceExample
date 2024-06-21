using ADMReestructuracion.Auth.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace ADMReestructuracion.Auth.DataAccess.Configuration
{
    public static class DataExtensions
    {
        public static void ConfigureDataService(this IServiceCollection services)
        {
            services.AddScoped<DbContext, AuthContext>();
            services.AddScoped<AuthContext>();
        }

        /// <summary>
        /// Metodo de extension para configurar el dbcontext
        /// </summary>
        /// <param name="builder"></param>
        //public static void ConfigureDatabase(this WebApplicationBuilder builder)
        //{
        //    var options = builder.Services.ConfiguracionDBcontext("EasyFCPC", builder.Configuration);
        //    builder.Services.AddDbContext<AuthContext>(options);

        //}
    }
}

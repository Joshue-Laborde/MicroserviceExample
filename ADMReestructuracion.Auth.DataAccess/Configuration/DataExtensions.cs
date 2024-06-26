using ADMReestructuracion.Auth.DataAccess.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        public static void ConfigureDatabase(this WebApplicationBuilder builder)
        {
            //var options = builder.Services.ConfiguracionDBcontext("ADM", builder.Configuration);
            //builder.Services.AddDbContext<AuthContext>(options);
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine($"Connection String: {connectionString}"); // Log para verificar la cadena de conexión
            builder.Services.AddDbContext<AuthContext>(options =>
                options.UseSqlServer(connectionString));

        }
    }
}

using System.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

namespace ADMReestructuracion.Common.Data.Extensions
{
    public static class DataExtensions
    {
        public static IEnumerable<T> ToList<T>(this DataTable table)
    where T : class
        {
            var json = JsonConvert.SerializeObject(table, Formatting.Indented);
            return JsonConvert.DeserializeObject<IEnumerable<T>>(json);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static EntityEntry SetProperty(this EntityEntry entry, string propertyName, object value)
        {
            try
            {
                if (entry != null && entry.Properties.Any(x => x.Metadata.Name == propertyName))
                {
                    var prop = entry.Property(propertyName);

                    if (prop != null) prop.CurrentValue = value;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return entry;
        }
        /// <summary>
        /// Metodo de extension sirve para poder registrar como un adicional la configuracion de los procedures
        /// </summary>
        /// <typeparam name="TDataContext"></typeparam>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="connString"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureDataContext<TDataContext, TService, TImplementation>(this IServiceCollection services, IConfiguration configuration, string connString)
            where TDataContext : DbContext
                 where TService : class
            where TImplementation : class, TService
        {
            services.AddScoped<TService, TImplementation>();
            services.AddScoped<TImplementation>();
            services.ConfigureDataContext<TDataContext>(configuration, connString);
            return services;
        }
        /// <summary>
        /// Metodo de extension parala inyeccion de dependencias y registro de la cadena de conexion del data access 
        /// </summary>
        /// <typeparam name="TDataContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="connString"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IServiceCollection ConfigureDataContext<TDataContext>(this IServiceCollection services, IConfiguration configuration, string connString)
        where TDataContext : DbContext
        {
            services.AddScoped<DbContext, TDataContext>();
            services.AddScoped<TDataContext>();

            if (string.IsNullOrEmpty(connString))
            {
                throw new ArgumentException("Referencia de conexión de la base de datos no establecida");
            }
            var options = services.ConfiguracionDBcontext(connString, configuration);
            services.AddDbContext<TDataContext>(options);
            return services;
        }
    }
}

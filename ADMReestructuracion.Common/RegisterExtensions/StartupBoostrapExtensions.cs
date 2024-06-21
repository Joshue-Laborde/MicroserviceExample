using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ADMReestructuracion.Common.RegisterExtensions
{
    public static class StartupBoostrapExtensions
    {
        /// <summary>
        /// Metodo Generico Para obtener datos del appsettings
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static TModel StartupBoostrap<TModel>(this IServiceCollection services, IConfiguration configuration)
            where TModel : class
        {
            var section = configuration.GetSection(typeof(TModel).Name);
            if (string.IsNullOrEmpty(section.Key))
            {
                return default;
            }
            services.Configure<TModel>(section);
            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<TModel>>();
            if (options != null && options.Value != null)
            {
                services.AddSingleton(options.Value);
                return options.Value;
            }
            return default;
        }
    }
}

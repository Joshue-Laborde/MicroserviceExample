using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ADMReestructuracion.Common.Data.Extensions
{
    public static class CommonDatabase
    {
        /// <summary>
        /// Metodo de extension para configurar el dbContext
        /// </summary>
        /// <param name="services"></param>
        /// <param name="name"></param>
        /// <param name="Configuration"></param>
        /// <returns></returns>
        public static Action<DbContextOptionsBuilder> ConfiguracionDBcontext(this IServiceCollection services, string name, IConfiguration Configuration)
        {
            Action<DbContextOptionsBuilder> option;
            var connectionString = Configuration.GetConnectionString(name);
            option = option =>
            {
                option.UseSqlServer(connectionString,
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure();
                        sqlOptions.CommandTimeout(120);
                    })
                .EnableDetailedErrors().EnableSensitiveDataLogging()
#if debug
.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
#endif
                ;
            };

            return option;
        }
    }
}

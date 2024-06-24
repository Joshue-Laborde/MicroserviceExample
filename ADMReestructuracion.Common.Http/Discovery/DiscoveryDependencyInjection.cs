using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ADMReestructuracion.Common.Http.Discovery
{
    public static class DiscoveryDependencyInjection
    {
        public static IServiceCollection AddDiscovery(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddSingleton<IConsulClient, ConsulClient>(provider => new ConsulClient(consulConfig =>
            {
                var address = configuration["Discovery:Address"];
                consulConfig.Address = new Uri(address);
            }))
                .AddSingleton<IServiceDiscovery, ConsulServiceDiscovery>();
        }
    }
}

using ADMReestructuracion.Common.Data.Repositories;
using ADMReestructuracion.Common.Http;
using ADMReestructuracion.Common.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ADMReestructuracion.Common.Startup.Dependencies
{
    public static class ExtensionServices
    {
        public static void ConfigureExtensionServices(this WebApplicationBuilder builder)
        {
            //builder.Services.AddTransient<IFileHandler, FileHandler>();
            // Inyeccion de servicios convensionales
            builder.Services.AddTransient(typeof(IEntityRepository<>), typeof(EntityRepository<>));
            builder.Services.AddTransient<ITools, Tools>();
            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Services.AddScoped<IValidatorFactory, ServiceProviderValidatorFactory>();
            //builder.Services.AddTransient<IHttpTools, HttpTools>();
            // PDF Tools
            //builder.Services.AddSingleton(typeof(DinkToPdf.Contracts.IConverter), new SynchronizedConverter(new PdfTools()));

        }
    }
}

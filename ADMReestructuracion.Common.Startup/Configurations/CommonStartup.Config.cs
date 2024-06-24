using ADMReestructuracion.Common.Extensions;
using ADMReestructuracion.Common.Http;
using ADMReestructuracion.Common.Http.Extensions;
using ADMReestructuracion.Common.Http.Filters;
using ADMReestructuracion.Common.Http.RegistersExtensions;
using ADMReestructuracion.Common.Http.Settings;
using ADMReestructuracion.Common.RegisterExtensions;
using ADMReestructuracion.Common.Startup.Dependencies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using System.Reflection;

namespace ADMReestructuracion.Common.Startup.Configurations
{
    public static partial class ComonStartup
    {

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        // This method gets called by the runtime. Use this method to add services to the container.
        public static ServiceSettings? ServiceSettings { get; set; }
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            //builder.AddAutenticationKeyVaultExtension();
            ServiceSettings = builder.Services.StartupBoostrap<ServiceSettings>(builder.Configuration);
            if (ServiceSettings != null)
            {
                builder.Services.AddConsulSettings(ServiceSettings);
            }
            //AzureIdentity identitySecret = builder.Services.StartupBoostrap<AzureIdentity>(builder.Configuration);
            //ExtensionEcryption.InicializateConfigColumn(identitySecret);


            string serviceName = builder.Configuration["ServiceName"] ?? string.Empty;
            //Assembly.GetFilteredReferencedAssemblies().GetPathcDocument();
            //builder.Services.AddSwaggerGen(swagger =>
            //{
            //    if (swagger != null && Assembly != null)
            //    {
            //        swagger.InsertPath(Assembly.GetFilteredReferencedAssemblies().GetPathcDocument());

            //    }
            //});

            // Configurar CORS
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            builder.ConfigureExtensionServices();
            builder.Services.ConfigureSwagger($"ADMReestructuracion - {serviceName} API");

            //builder.Services.ConfigureAuth(builder.Configuration);
            builder.Services.AddHeaderPropagation();
            // builder.Services.ConfigureReporting();
            builder.Services.ConfigureControllers();

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            builder.Services.AddLogging();
            //builder.Services.AddApplicationInsightsTelemetry();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(this WebApplication app)
        {
            //app.UseApplicationInsightsExceptionTelemetry();
            //app.UseApplicationInsightsRequestTelemetry();

            string serviceName = "";
            if (ServiceSettings != null)
            {
                app.UseConsul(ServiceSettings);

            }

            serviceName = app.Configuration["ServiceName"] ?? string.Empty;
            app.Initialize($"{serviceName} API", app.Environment, app.Configuration);
            // app.UseMiddleware<MiJsonSerializationException>();

        }

        public static void InitializeMiddleware(this WebApplication app)
        {
            app.UseExceptionMiddleware();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
            ControllerBase.Initialize(app.Services, app.Configuration, app.Environment);
            BusinessExtensions.Initialize(app.Services);

        }

        public static void InitializeHub<THub>(this WebApplication app, string path)
            where THub : Hub
        {
            app.UseExceptionMiddleware();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<THub>(path);
                endpoints.MapControllers();
            });

            ControllerBase.Initialize(app.Services, app.Configuration, app.Environment);
            BusinessExtensions.Initialize(app.Services);

        }
    }
}

using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ADMReestructuracion.Common.Startup.Configurations
{
    public static partial class ComonStartup
    {
        public static Assembly? Assembly { get; set; }
        public static WebApplication Create(Action<WebApplicationBuilder>? webappBuilder = null)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder();
            builder.ConfigureServices();
            webappBuilder?.Invoke(builder);
            return builder.Build();
        }
        public static void RunStart(this WebApplication app)
        {
            app.Configure();
            //app.UseMiddleware<AuthMiddleware>();
            app.InitializeMiddleware();
            app.Run();
        }

        public static WebApplication RunStart(this WebApplication app, Action<WebApplication>? appinvoke = null)
        {
            app.Configure();
            appinvoke?.Invoke(app);
            app.InitializeMiddleware();
            return app;
        }
    }
}

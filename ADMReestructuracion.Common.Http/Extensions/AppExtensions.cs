using ADMReestructuracion.Common.Operations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ADMReestructuracion.Common.Http.Extensions
{
    public static class AppExtensions
    {
        public static void Initialize(this IApplicationBuilder app, string title, IWebHostEnvironment env, IConfiguration settings)
        {
            var routePrefix = settings["RoutePrefix"] ?? "";

            app.UseCors("allReady");

            app.UseDeveloperExceptionPage();

            app.UseSwagger();
            //  
            app.UseSwaggerUI(options =>
            {
                var swaggerEndpoint = $"{routePrefix}/swagger/v1/swagger.json";
                options.SwaggerEndpoint(swaggerEndpoint, $"{title} {env?.EnvironmentName} v1");
                //options.SwaggerEndpoint("/swagger/v1/swagger.json", $"{title} {env?.EnvironmentName} v1");
                options.RoutePrefix = string.Empty;
            });

            app.UseStatusCodePagesWithReExecute($"{routePrefix}/error/{0}");
            app.UseExceptionHandler($"{routePrefix}/error");
            app.UseHeaderPropagation();
            app.UseHttpsRedirection();
            app.UseRouting();
            //app.UseAuthentication();
            app.UseStatusCodePages(async context => await context.HttpContext.Response.WriteAsJsonAsync(new OperationResult((HttpStatusCode)context.HttpContext.Response.StatusCode)));
            app.UseHeaderPropagation();

        }
    }
}

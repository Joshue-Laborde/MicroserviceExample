using ADMReestructuracion.Common.Operations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace ADMReestructuracion.Common.Http.Extensions
{
    public static class AppExtensions
    {
        public static void Initialize(this IApplicationBuilder app, string title, IWebHostEnvironment env, IConfiguration settings)
        {
            var routePrefix = settings["RoutePrefix"] ?? "";

            app.UseRouting();

            app.UseCors("AllowSpecificOrigin");

            app.UseSwagger();           
            app.UseSwaggerUI(options =>
            {
                var swaggerEndpoint = $"{routePrefix}/swagger/v1/swagger.json";
                options.SwaggerEndpoint(swaggerEndpoint, $"{title} {env?.EnvironmentName} v1");
                //options.SwaggerEndpoint("/swagger/v1/swagger.json", $"{title} {env?.EnvironmentName} v1");
                options.RoutePrefix = string.Empty;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler($"{routePrefix}/error");
                app.UseStatusCodePagesWithReExecute($"{routePrefix}/error/{0}");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseHeaderPropagation();
            //app.UseAuthentication();
            app.UseStatusCodePages(async context =>
            {
                await context.HttpContext.Response.WriteAsJsonAsync(new OperationResult((HttpStatusCode)context.HttpContext.Response.StatusCode));
            });

        }
    }
}

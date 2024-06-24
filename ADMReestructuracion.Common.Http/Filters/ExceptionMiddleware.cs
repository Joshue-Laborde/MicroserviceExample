using ADMReestructuracion.Common.Operations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace ADMReestructuracion.Common.Http.Filters
{
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                // Manejar la excepción aquí
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var mensaje = exception.Message;
            var error = exception.StackTrace;
            var response = new OperationResult(System.Net.HttpStatusCode.InternalServerError, $"hubo un Problema al responder el resultado observe: {mensaje}", error);
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            // Serializar la respuesta de error como JSON y escribirla en la respuesta
            var json = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(json);
        }
    }
}

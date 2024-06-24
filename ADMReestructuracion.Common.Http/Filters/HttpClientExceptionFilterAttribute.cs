using ADMReestructuracion.Common.Operations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;


public class HttpClientExceptionFilterAttribute : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        try
        {
            var result = new OperationResult(context.Exception);
            context.Result = new JsonResult(result);
            context.HttpContext.Response.StatusCode = (int)result.StatusCode;
        }
        catch (Exception ex)
        {
            var result = new OperationResult(ex);
            context.Result = new JsonResult(result);
            context.HttpContext.Response.StatusCode = (int)result.StatusCode;
        }

        //   base.OnException(context);
    }


}


public class MiJsonSerializationException : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (JsonSerializationException ex)
        {
            // Maneja la excepción aquí
            var result = new OperationResult(ex);
            context.Response.StatusCode = (int)result.StatusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
        }
    }
}


public class HttpClientExceptionFil : IAsyncExceptionFilter
{
    public async Task OnExceptionAsync(ExceptionContext context)
    {
        try
        {
            var result = new OperationResult(context.Exception);
            context.Result = new JsonResult(result);
            context.HttpContext.Response.StatusCode = (int)result.StatusCode;
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            var result = new OperationResult(ex);
            context.Result = new JsonResult(result);
            context.HttpContext.Response.StatusCode = (int)result.StatusCode;
            await Task.CompletedTask;
        }
    }
}

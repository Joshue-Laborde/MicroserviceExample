using ADMReestructuracion.Common.Operations;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ADMReestructuracion.Common.Http.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //before controller


            if (!context.ModelState.IsValid)
            {
                var errorsInModelState = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(x => x.ErrorMessage).ToArray());



                var errorResponse = new OperationResult(System.Net.HttpStatusCode.BadRequest, "Los datos especificados son incorrectos");



                foreach (var error in errorsInModelState)
                {
                    errorResponse.Error = $"{error.Key}: f {string.Join("\r\n", error.Value)}";
                    context.Result = new BadRequestObjectResult(errorResponse);
                    return;
                }



                await next();



                //after controller  
            }
        }
    }
    public static class ExtensionFilter
    {
        public static BadRequestObjectResult GenerateBodyError(this ActionContext context)
        {

            //   var modelstate = context.ModelState.();
            OperationResult errorResponse = new OperationResult();
            if (!context.ModelState.IsValid)
            {
                var errorsInModelState = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(x => x.ErrorMessage).ToArray());

                string error = "";
                foreach (var errores in errorsInModelState)
                {
                    var mensajeError = $"{errores.Key}:  {string.Join("\r\n", errores.Value)}";
                    error = string.IsNullOrEmpty(error) ? mensajeError : $"{error} {mensajeError}";
                }

                var keys = string.Join(", ", errorsInModelState.Keys);

                errorResponse = new OperationResult(System.Net.HttpStatusCode.BadRequest, $"Los datos especificados son incorrectos {keys}", error);
                //after controller  
            }
            BadRequestObjectResult badRequestObjectResult = new BadRequestObjectResult(errorResponse)
            {
                ContentTypes =
                {
                    // using static System.Net.Mime.MediaTypeNames;
                    Application.Json,
                    Application.Xml
                },
            };
            return badRequestObjectResult;
        }
    }
}

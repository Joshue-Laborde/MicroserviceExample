using ADMReestructuracion.Common.Extensions;
using ADMReestructuracion.Common.Http.Models;
using ADMReestructuracion.Common.Interfaces;
using ADMReestructuracion.Common.Operations;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ADMReestructuracion.Common.Http
{
    public abstract class ControllerBase : Controller, IOperationRequest
    {
        protected static IConfiguration Settings { get; private set; }
        protected static ITools Tools { get; private set; }
        protected static IMapper Mapper { get; private set; }
        protected static IWebHostEnvironment HostingEnvironment { get; private set; }
        //protected string ResourcesPath => Settings["Resources"];

        public static void Initialize(IServiceProvider services, IConfiguration config, IWebHostEnvironment hostingEnvironment) //, IWebHostEnvironment hostingEnvironment)
        {
            Settings = config;
            HostingEnvironment = hostingEnvironment;
            Mapper = services.GetService<IMapper>();
            Tools = services.GetService<ITools>();
        }

        public IUserEntity Usuario => (IUserEntity)HttpContext.Items["User"] ?? new UsuarioDto();

        public ICompanyEntity Empresa => (ICompanyEntity)HttpContext.Items["Empresa"] ?? new EmpresaDto();

        public string Ip => RequestIP?.ToString() ?? "";

        public DateTime Fecha => DateTime.Now;

        public int IdEmpresa => Empresa?.IdEmpresa ?? 1;

        public string InicioSesion => Usuario?.CodigoUsuario ?? "SYSTEM";

        public IPAddress RequestIP
        {
            get
            {
                var remoteIpAddress = HttpContext.Connection?.RemoteIpAddress;

                return remoteIpAddress;
            }
        }

        //
        // Summary:
        //     Creates a Microsoft.AspNetCore.Mvc.ContentResult object by specifying a HTML template file and model
        //
        // Parameters:
        //   content:
        //     The content to write to the response.
        //
        //   contentType:
        //     The content type (MIME type).
        //
        // Returns:
        //     The created Microsoft.AspNetCore.Mvc.ContentResult object for the response.
        //[NonAction]
        //protected virtual ContentResult HTML<T>(string template, T model)
        //{
        //    return Content(Tools.GetHtmlTemplate(template, model), "text/html");
        //}

        [NonAction]
        protected virtual ObjectResult Error(Exception ex, string message = null)
        {
            var result = new OperationResult(ex)
            {
                Message = message ?? ex.Message
            };

            return StatusCode(500, result);
        }

        [NonAction]
        protected virtual ObjectResult Error(HttpStatusCode statusCode, string message, string exception)
        {
            return StatusCode((int)statusCode, new OperationResult(statusCode, message, exception));
        }

        [NonAction]
        protected virtual ObjectResult Error(IOperationResult result)
        {
            return StatusCode((int)result.StatusCode, result);
        }

        [NonAction]
        protected virtual ObjectResult StatusCode(HttpStatusCode statusCode, object result)
        {
            return StatusCode((int)statusCode, result);
        }

        [NonAction]
        protected virtual ObjectResult StatusCode<T>(IOperationResult<T> result)
        {
            return StatusCode((int)result.StatusCode, result);
        }
    }


    public static class CommonExtensions
    {
        public static ObjectResult ToObjectResult<T>(this IOperationResult<T> result) where T : class
        {
            return new ObjectResult(result) { StatusCode = (int)result.StatusCode };
        }

        public static ObjectResult ToObjectResult(this IOperationResult result)
        {
            try
            {
                return new ObjectResult(result) { StatusCode = (int)result.StatusCode };

            }
            catch (Exception ex)
            {

                return new ObjectResult(null) { StatusCode = (int)result.StatusCode };

            }

        }

        public static ObjectResult ToObjectResult(this Exception ex, string message = null)
        {
            var result = ex.ToResult();
            if (!string.IsNullOrEmpty(message))
            {
                result.Message = message;
            }

            return new ObjectResult(result) { StatusCode = (int)result.StatusCode };
        }
    }
}

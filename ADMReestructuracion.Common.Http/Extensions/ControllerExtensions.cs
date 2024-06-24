using ADMReestructuracion.Common.Http.Filters;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ADMReestructuracion.Common.Http.Extensions
{
    public static class ControllerExtensions
    {
        public static void ConfigureControllers(this IServiceCollection services)
        {


            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });
            services.AddControllersWithViews(options =>
            {
                //options.Filters.Add<ApiAuthorizeAttribute>();
                options.Filters.Add<HttpClientExceptionFilterAttribute>();
            }).ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                  context.GenerateBodyError();
            })

          .AddJsonOptions(options =>
          {

              options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
              options.JsonSerializerOptions.WriteIndented = true;
              options.JsonSerializerOptions.AllowTrailingCommas = true;
              options.JsonSerializerOptions.IgnoreNullValues = true;
              options.JsonSerializerOptions.UnknownTypeHandling = JsonUnknownTypeHandling.JsonNode;
              options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
              options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
              //options.JsonSerializerOptions.Encoder=System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
              //options.JsonSerializerOptions.PropertyNamingPolicy=JsonNamingPolicy.CamelCase;
              //  options.JsonSerializerOptions.Converters.Add(new DecimalFormatJsonConverter());


              //options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.AllowNamedFloatingPointLiterals;


          }
           )


          //.AddNewtonsoftJson(option =>
          // {
          //     option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
          //     option.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
          //     option.ReadJsonWithRequestCulture = true;
          //     option.SerializerSettings.FloatParseHandling = Newtonsoft.Json.FloatParseHandling.Decimal;

          //     option.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

          // })

          .ConfigureApiBehaviorOptions(options =>
          {
              options.SuppressModelStateInvalidFilter = false;
          }

          );

        }
    }
}

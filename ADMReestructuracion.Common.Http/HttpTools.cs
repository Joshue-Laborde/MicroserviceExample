using ADMReestructuracion.Common.Enums;
using ADMReestructuracion.Common.Http.Filters;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Collections;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Web;

using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ADMReestructuracion.Common.Http
{
    public static class HttpToolsBuilder
    {
        public static void ConfigureRouting(this IServiceCollection services, IConfiguration configuration = null)
        {
            configuration ??= services.BuildServiceProvider().GetService<IConfiguration>();

            var serviceUrls = configuration.GetSection("ApiServices")
                                           .GetChildren()
                                           .ToDictionary(section => section.Key, section => section.Value);

            foreach (var serviceUrlEntry in serviceUrls)
            {
                services.ConfigureRouting(serviceUrlEntry.Key, serviceUrlEntry.Value);
            }

            services.AddTransient<IHttpTools, HttpTools>();
        }

        public static void ConfigureRouting(this IServiceCollection services, string clientName, string url)
        {
            services.AddHttpClient(clientName, options =>
            {
                options.BaseAddress = new Uri(url);
            }).AddHeaderPropagation();

            //services.AddHeaderPropagation(options =>
            //{
            //    options.Headers.Add("Authorization", x =>
            //    {
            //        return x.HeaderValue;
            //    });
            //});
        }
    }

    public class HttpTools : IHttpTools
    {
        private IHttpClientFactory http;
        private IConfiguration configuration;
        private HttpClient httpClient;
        public Dictionary<string, object> ParametersQuery { set; get; }

        public HttpTools(IHttpClientFactory http, IConfiguration configuration)
        {
            this.http = http;
            this.configuration = configuration;
            ParametersQuery = new Dictionary<string, object>();
            httpClient = http.CreateClient("ipsp-produccion");
        }


        public void ConfigureService(string service)
        {
            if (!string.IsNullOrEmpty(service))
            {
                httpClient = http.CreateClient("ipsp-produccion");
            }


        }
        private async Task<StringBuilder> InsertQuery()
        {
            var queryString = new StringBuilder();

            await ParametersQuery.ToList().ForEachAsync(async item =>
            {
                if (queryString.Length > 0) queryString.Append('&');
                queryString.AppendFormat("{0}={1}", item.Key, HttpUtility.UrlEncode($"{item.Value}"));
            });


            return queryString;
        }
        public string BasicAutentication { get; private set; }
        private string GetCretentials()
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(configuration["RouteApi:username"] + ":" + configuration["RouteApi:password"]));

        }

        public async Task<HttpResponseMessage> EvalueMethtpp(Methttp methttp, string url, HttpContent httpContent, HttpClient client)
        {
            HttpResponseMessage response = new();
            return methttp switch
            {
                Methttp.POST => response = await client.PostAsync(url, httpContent),
                Methttp.GET => response = await client.GetAsync(url),
                Methttp.PUT => response = await client.PutAsync(url, httpContent),
                Methttp.DELETTE => response = await client.DeleteAsync(url),
                Methttp.PATCH => response = await client.PatchAsync(url, httpContent),
                Methttp.UNDEFINED => null,
            };

        }
        public async Task<TResul> GetDataHttp<TResul>(string url, Methttp metodohtp = Methttp.GET, object objeto = null, string remplace = null) where TResul : class, new()
        {
            try
            {
                TResul resultado = new TResul();

                if (remplace != null)
                {

                    url = url.Replace("{0}", remplace);
                }
                StringContent httpContents = null;

                if (objeto != null)
                {
                    string jsonString = JsonSerializer.Serialize(objeto);

                    httpContents = new StringContent(jsonString, Encoding.UTF8, "application/json");

                }



                var queryString = await InsertQuery();
                httpClient = http.CreateClient("Api");
                // httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", BasicAutentication);
                httpClient.Timeout = TimeSpan.FromMinutes(5);



                url = httpClient.BaseAddress + $"{url}?{queryString}";

                var result = await EvalueMethtpp(metodohtp, url, httpContents, httpClient);
                ParametersQuery.Clear();
                if (result != null && result.IsSuccessStatusCode)
                {

                    var resultadso = await result.Content.ReadAsStringAsync();
                    JsonDocument jsonDocument = JsonDocument.Parse(resultadso);
                    //validar si la resuesta es un array
                    if (jsonDocument.RootElement.ValueKind == JsonValueKind.Array)
                    {
                        // validar si el modelo es del mismo tipo de objeto
                        if (resultado is IList || typeof(IEnumerable).IsAssignableFrom(resultado.GetType()) && resultado.GetType() != typeof(string))
                        {

                            resultado = JsonConvert.DeserializeObject<TResul>(resultadso);
                            return resultado;
                        }
                        return null;
                    }

                    resultado = JsonConvert.DeserializeObject<TResul>(resultadso);
                    //   resultado = await result.Content.ReadFromJsonAsync<TResul>();
                    return resultado;
                }
                if (result.StatusCode == HttpStatusCode.BadRequest)
                {
                    return null;
                }
                var content = await result.Content.ReadAsStringAsync();

                throw new Exception(content);
            }
            catch (Exception ex)
            {
                string error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                error = error + $"Route:{url}";
                throw new Exception(error, ex);
            }

        }


    }
}

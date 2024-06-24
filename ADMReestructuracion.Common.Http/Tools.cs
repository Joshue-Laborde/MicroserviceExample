using ADMReestructuracion.Common.Enums;
using ADMReestructuracion.Common.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace ADMReestructuracion.Common.Http
{
    public class Tools : ITools
    {
        //public string ResourcesPathUri => _config["Resources"];
        //public string ResourcesPath => ResourcesPathUri?.Replace("/", "\\");

        protected static IWebHostEnvironment HostingEnvironment { get; private set; }

        private readonly IConfiguration _config;
        //private readonly IConverter _converter;
        private readonly ILogger<Tools> _logger;
        //private readonly IFileHandler _storage;

        public Tools(IWebHostEnvironment hostingEnvironment, IConfiguration config, ILogger<Tools> logger)
        {
            HostingEnvironment = hostingEnvironment;
            //_converter = converter;
            _config = config;
            _logger = logger;
            //_storage = storage;
        }

        protected static HttpClient client { set; get; }
        protected static HttpContent content { set; get; }
        protected static HttpResponseMessage response { set; get; }

        public async Task<string> SerializarObjectjson(object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HttpResponseMessage> EvalueMethtpp(Methttp methttp, string url, HttpContent httpContent, HttpClient client)
        {
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

        public async Task<T> DeserializarJsonAobjeto<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }

        public async Task<T> PostJsonAllAscyn<T>(Methttp methttp, string url, string acces_token = null, string Schema = null, object obj = null)
        {
            try
            {
                string jsonObj = string.Empty;

                T resultObj = default;
                if (obj != null)
                {
                    jsonObj = await SerializarObjectjson(obj);

                }


                client = new System.Net.Http.HttpClient();
                client.Timeout = TimeSpan.FromSeconds(30);
                if (acces_token != null)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Schema ?? "Bearer", acces_token);
                }

                StringContent httpContent = new StringContent(jsonObj, Encoding.UTF8, "application/json");
                HttpResponseMessage response;
                response = await EvalueMethtpp(methttp, url, content, client);

                if (response != null)
                {
                    content = response.Content;
                    var a = await content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(a))
                    {
                        //por hacer
                    }
                    resultObj = await DeserializarJsonAobjeto<T>(a);
                    if (resultObj == null) throw new Exception($"Response null For Service{response.StatusCode} ");
                    return resultObj;
                }


                return default;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }

        private bool IsProductionEnvironment()
        {
            // Lógica para verificar si el entorno es producción.
            // Puedes utilizar variables de entorno, configuraciones específicas o cualquier otra técnica
            // para determinar si el código se está ejecutando en producción.

            // Ejemplo de verificación basada en una variable de entorno
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            return environment == "Production";
        }

        public bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        public bool VerificaIdentificacion(string identificacion, bool cedula = false)
        {
            bool estado = false;

            if (identificacion.Length == 10 && cedula == true || identificacion.Length == 13 && cedula == false)
            {
                var valced = identificacion.Trim().ToCharArray();
                var provincia = int.Parse(valced[0].ToString() + valced[1].ToString());

                if (provincia > 0 && provincia < 25)
                {
                    if (int.Parse(valced[2].ToString()) < 6)
                    {
                        estado = VerificaCedula(new string(valced));
                    }
                    else if (int.Parse(valced[2].ToString()) == 6)
                    {
                        estado = VerificaSectorPublico(new string(valced));
                    }
                    else if (int.Parse(valced[2].ToString()) == 9)
                    {
                        estado = VerificaPersonaJuridica(new string(valced));
                    }
                }
            }

            return estado;
        }

        public bool VerificaCedula(string validarCedula)
        {
            if (validarCedula.Length != 13 && validarCedula.Length != 10)
                return false;

            int aux = 0, par = 0, impar = 0, verifi;
            for (int i = 0; i < 9; i += 2)
            {
                aux = 2 * int.Parse(validarCedula[i].ToString());
                if (aux > 9)
                    aux -= 9;
                par += aux;
            }
            for (int i = 1; i < 9; i += 2)
            {
                impar += int.Parse(validarCedula[i].ToString());
            }

            aux = par + impar;
            if (aux % 10 != 0)
            {
                verifi = 10 - aux % 10;
            }
            else
                verifi = 0;
            if (verifi == int.Parse(validarCedula[9].ToString()))
                return true;
            else
                return false;
        }

        public bool VerificaPersonaJuridica(string validarCedula)
        {
            if (validarCedula.Length != 13)
                return false;

            int aux = 0, prod, veri;
            veri = int.Parse(validarCedula[10].ToString()) + int.Parse(validarCedula[11].ToString()) + int.Parse(validarCedula[12].ToString());
            if (veri > 0)
            {
                int[] coeficiente = new int[9] { 4, 3, 2, 7, 6, 5, 4, 3, 2 };
                for (int i = 0; i < 9; i++)
                {
                    prod = int.Parse(validarCedula[i].ToString()) * coeficiente[i];
                    aux += prod;
                }
                if (aux % 11 == 0)
                {
                    veri = 0;
                }
                else if (aux % 11 == 1)
                {
                    return false;
                }
                else
                {
                    aux = aux % 11;
                    veri = 11 - aux;
                }

                if (veri == int.Parse(validarCedula[9].ToString()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }




        public bool VerificaSectorPublico(string validarCedula)
        {
            if (validarCedula.Length != 13)
                return false;

            int aux = 0, prod, veri;
            veri = int.Parse(validarCedula[9].ToString()) + int.Parse(validarCedula[10].ToString()) + int.Parse(validarCedula[11].ToString()) + int.Parse(validarCedula[12].ToString());
            if (veri > 0)
            {
                int[] coeficiente = new int[8] { 3, 2, 7, 6, 5, 4, 3, 2 };

                for (int i = 0; i < 8; i++)
                {
                    prod = int.Parse(validarCedula[i].ToString()) * coeficiente[i];
                    aux += prod;
                }

                if (aux % 11 == 0)
                {
                    veri = 0;
                }
                else if (aux % 11 == 1)
                {
                    return false;
                }
                else
                {
                    aux = aux % 11;
                    veri = 11 - aux;
                }

                if (veri == int.Parse(validarCedula[8].ToString()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

    }

    public static class ToolStatic
    {

        public static JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            // Utilizamos las mismas opciones configuradas en AddJsonOptions.
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            AllowTrailingCommas = true,
            IgnoreNullValues = true,
            UnknownTypeHandling = JsonUnknownTypeHandling.JsonNode,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            Converters = { new JsonStringEnumConverter() }
        };
        public static bool IsNumericType(this Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return true;
                default:
                    return false;
            }
        }

        public static async Task ForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> action)
        {
            await ForEachAsync<T>(enumerable, action, true);
        }

        public static async Task<IFormFile> ConvertToIFormFile(this byte[] data, string fileName, string contentType)
        {
            using (var ms = new MemoryStream(data))
            {
                var file = new FormFile(ms, 0, data.Length, fileName, fileName)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = contentType
                };
                await Task.CompletedTask;
                return await Task.FromResult(file);

            }
        }

        public static async Task<byte[]> ConvertIFormFileToByteArray(this IFormFile formFile)
        {
            using (var ms = new MemoryStream())
            {
                formFile.CopyTo(ms);
                await Task.CompletedTask;
                return await Task.FromResult(ms.ToArray());
            }
        }
        public static async Task ForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> action, bool boll = true)
        {
            foreach (var item in enumerable)
            {
                if (boll == true)
                {
                    await action(item);
                }
                else
                {
                    break;
                }

            }
        }
    }

}


using ADMReestructuracion.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADMReestructuracion.Common.Http.Filters
{
    public interface IHttpTools
    {
        public Task<TResul> GetDataHttp<TResul>(string url, Methttp metodohtp = Methttp.GET, object objeto = null, string remplace = null) where TResul : class, new();
        public Dictionary<string, object> ParametersQuery { set; get; }
        public void ConfigureService(string service);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADMReestructuracion.Common.Http.Settings
{
    public class ServiceSettings
    {
        public string ServiceName { get; set; }
        public string ServiceHost { get; set; }
        public int ServicePort { get; set; }
        public string ServiceDiscoveryAddress { get; set; }
    }
}

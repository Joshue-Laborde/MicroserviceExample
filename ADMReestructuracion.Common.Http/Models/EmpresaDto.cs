using ADMReestructuracion.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADMReestructuracion.Common.Http.Models
{
    public class EmpresaDto : ICompanyEntity
    {
        public int IdEmpresa { get; set; }
        public string Ruc { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public bool Activo { get; set; }
    }
}

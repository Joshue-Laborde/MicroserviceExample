using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADMReestructuracion.Common.Interfaces
{
    public interface ICompanyEntity
    {
        int IdEmpresa { get; set; }
        string Ruc { get; set; }
        string Nombre { get; set; }
        string Telefono { get; set; }
        string Direccion { get; set; }
        bool Activo { get; set; }
    }
}

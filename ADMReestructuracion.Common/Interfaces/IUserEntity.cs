using ADMReestructuracion.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADMReestructuracion.Common.Interfaces
{
    public interface IUserEntity
    {
        int IdUsuario { get; set; }
        string Nombres { get; set; }
        string Apellidos { get; set; }
        string correo { get; set; }
        string clave { get; set; }
        string Token { get; set; }
        bool Activo { get; set; }
        ICompanyEntity Empresa { get; }

    }

    public interface IAplicationEntity
    {
        public int IdAplicacion { get; set; }
        public string Descripcion { get; set; }
        public TipoAplicacionEnum Tipo { get; set; }
        //public Guid Token { get; set; }
        public bool Activo { get; set; }
    }
}

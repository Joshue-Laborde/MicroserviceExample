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
        public int IdUsuario { get; set; }

        public int IdTipoIdentificacion { get; set; }

        public int? IdTipoSangre { get; set; }

        public string Nombres { get; set; }

        public string Apellidos { get; set; }

        public string Correo { get; set; }

        public string CodigoUsuario { get; set; }

        //public string Clave { get; set; }

        public bool? Ad { get; set; }

        public bool? Qr { get; set; }

        public string RutaQr { get; set; }

        public bool Activo { get; set; }
        //ICompanyEntity Empresa { get; }

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

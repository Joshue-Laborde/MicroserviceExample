using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADMReestructuracion.Auth.Domain.Models
{
    public class UsuarioDto
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

        //public DateTime? FechaRegistro { get; set; }

        //public string UsuarioRegistro { get; set; }

        //public DateTime? FechaModificacion { get; set; }

        //public string UsuarioModificacion { get; set; }

        public string Identificacion { get; set; }
    }
}

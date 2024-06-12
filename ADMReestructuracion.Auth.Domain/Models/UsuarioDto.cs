using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADMReestructuracion.Auth.Domain.Models
{
    public class UsuarioDto
    {
        public string Correo { get; set; }

        public string Clave { get; set; }

        public DateTime FechaCreacion { get; set; }

        public bool IdEstado { get; set; }
    }
}

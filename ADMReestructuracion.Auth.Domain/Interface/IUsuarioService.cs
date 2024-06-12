using ADMReestructuracion.Auth.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADMReestructuracion.Auth.Domain.Interface
{
    public interface IUsuarioService
    {
        Task<UsuarioDto> GetUsuario();
    }
}

using ADMReestructuracion.Common.Http.Models;
using ADMReestructuracion.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ADMReestructuracion.Common.Http.Models
{
    internal class UsuarioDto
    {
    }
}
internal class UsuarioDto : IUserEntity
{
    public UsuarioDto()
    {
        Nombres = "SYSTEM USER";
        Correo = "app@ipsp-produccion.com";
        CodigoUsuario = "SYSTEM";
        //Token = $"{Guid.Empty}";
        //Roles = Array.Empty<string>();
    }

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

}

internal class AuthResult : IOperationResult<UsuarioDto>
{
    public UsuarioDto Result { get; set; }
    public string Error { get; set; }
    public string Message { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public bool Success { get; set; }
}

internal class EmpresaResult : IOperationResult<EmpresaDto>
{
    public EmpresaDto Result { get; set; }
    public string Error { get; set; }
    public string Message { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public bool Success { get; set; }
}
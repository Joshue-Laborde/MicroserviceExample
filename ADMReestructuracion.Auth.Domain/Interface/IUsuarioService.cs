using ADMReestructuracion.Auth.Domain.Models;
using ADMReestructuracion.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADMReestructuracion.Auth.Domain.Interface
{
    public interface IUsuarioService
    {
        Task<IOperationResultList<UsuarioDto>> GetUsers(int page = 1, int? pageSize = 10);
        Task<IOperationResult<UsuarioDto>> Login(string name, string password);
    }
}

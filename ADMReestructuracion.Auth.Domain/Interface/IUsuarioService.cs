using ADMReestructuracion.Auth.Domain.Models;
using ADMReestructuracion.Common.Interfaces;

namespace ADMReestructuracion.Auth.Domain.Interface
{
    public interface IUsuarioService
    {
        Task<IOperationResult<UsuarioDto>> Login(string name, string password);
        Task<IOperationResult<UsuarioDto>> GetUserById(string term);
        Task<IOperationResultList<UsuarioDto>> GetUsers(int page = 1, int? pageSize = 10);
    }
}

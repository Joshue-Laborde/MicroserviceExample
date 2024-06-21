using ADMReestructuracion.Auth.DataAccess.Models;
using ADMReestructuracion.Auth.DataAccess.Repositories;
using ADMReestructuracion.Auth.Domain.Interface;
using ADMReestructuracion.Auth.Domain.Models;
using ADMReestructuracion.Common.Extensions;
using ADMReestructuracion.Common.Interfaces;
using ADMReestructuracion.Common.Operations;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace ADMReestructuracion.Auth.BusinessLogic.Service
{
    public class UsuarioService : IUsuarioService
    {

        private readonly IEntityRepository<GenUsuario> _usuario;
        private readonly IMapper _mapper;

        public UsuarioService(IEntityRepository<GenUsuario> usuario, IMapper mapper)
        {
            _usuario = usuario;
            _mapper = mapper;
        }
        public async Task<IOperationResultList<UsuarioDto>> GetUsers(int page = 1, int? pageSize = 10)
        {
            try
            {
                var users = _usuario.Search(x => x.Activo == true && x.CodigoUsuario.Length > 1);
                //var result = _mapper.Map<List<UsuarioDto>>(users);

                return await users.ToResultListAsync<GenUsuario,UsuarioDto>(page,pageSize);

            }
            catch (Exception e)
            {
                string error = e.InnerException != null ? e.InnerException.Message : e.Message;
                return new OperationResultList<UsuarioDto>(HttpStatusCode.InternalServerError, "Ha ocurrido un problema en el servidor", null,page,pageSize,null);
            }
        }

        public async Task<IOperationResult<UsuarioDto>> Login(string name, string password)
        {
            try
            {
                var user = await _usuario.Search(x => x.Activo == true && x.CodigoUsuario == name && x.Clave == GetSHA256(password)).FirstOrDefaultAsync();

                if (user == null)
                {
                    return new OperationResult<UsuarioDto>(HttpStatusCode.NotFound, "Usuario no encontrado");
                }

                var result = _mapper.Map<UsuarioDto>(user);

                return await result.ToResultAsync();

            }
            catch (Exception e)
            {
                string error = e.InnerException != null ? e.InnerException.Message : e.Message;
                return new OperationResult<UsuarioDto>(HttpStatusCode.InternalServerError, "Ha ocurrido un problema en el servidor", null, error);
            }

        }


        public static string GetSHA256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                {
                    sb.AppendFormat("{0:x2}", b);
                }

                return sb.ToString();
            }
        }

    }
}

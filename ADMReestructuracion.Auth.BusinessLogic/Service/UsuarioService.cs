using ADMReestructuracion.Auth.DataAccess.Repositories;
using ADMReestructuracion.Auth.Domain.Interface;
using ADMReestructuracion.Auth.Domain.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADMReestructuracion.Auth.BusinessLogic.Service
{
    public class UsuarioService : IUsuarioService
    {

        private readonly AuthContext _context;
        private readonly IMapper _mapper;

        public UsuarioService(AuthContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }
        public async Task<UsuarioDto> GetUsuario()
        {
            try
            {
                var usuario = await _context.Usuario.Where(x => x.IdEstado).ToListAsync();
                var result = _mapper.Map<UsuarioDto>(usuario);

                return result;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener clientes: {ex.Message}");
                return new UsuarioDto();
            }
        }
    }
}

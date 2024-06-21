using ADMReestructuracion.Auth.Domain.Interface;
using ADMReestructuracion.Auth.Domain.Models;
using ADMReestructuracion.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ADMReestructuracion.Auth.Api.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public AuthController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        /// <summary>
        /// logeo de usuario
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(string name, string password)
        {
            try
            {
                var result = await _usuarioService.Login(name, password);

                if (result == null) return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Obtiene todos los usuarios
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("usuarios")]
        [ProducesResponseType(typeof(IOperationResult<List<UsuarioDto>>), 200)]
        [ProducesResponseType(typeof(IOperationResult), 404)]
        public async Task<IActionResult> GetUsers(int page = 1, int? pageSize = 10)
        {
            try
            {
                var users = await _usuarioService.GetUsers(page, pageSize);

                if (users == null) return NotFound();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}

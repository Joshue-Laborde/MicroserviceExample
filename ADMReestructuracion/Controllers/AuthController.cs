using ADMReestructuracion.Auth.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ADMReestructuracion.Auth.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public AuthController(IUsuarioService usuarioService)
        {
            this._usuarioService = usuarioService;
        }

        [HttpGet]
        [Route("usuarios")]
        public async Task<IActionResult> GetUsuario()
        {
            try
            {
                var usuario = await _usuarioService.GetUsuario();

                if (usuario == null) return NotFound();

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

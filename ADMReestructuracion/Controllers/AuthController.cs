using ADMReestructuracion.Auth.Domain.Interface;
using ADMReestructuracion.Auth.Domain.Models;
using ADMReestructuracion.Common.Interfaces;
using ADMReestructuracion.Common.Http.Extensions;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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

                // Crear claims para el usuario autenticado
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, name),
                    new Claim(ClaimTypes.Email, result.Result.Correo ?? "app@ipsp-produccion.com"),
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Crear cookie de autenticación
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                });

                // Almacenar información adicional en la sesión
                HttpContext.Session.SetObject("Usuario", result.Result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Desloguearte
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            // Eliminar la cookie de autenticación
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Limpiar la sesión
            HttpContext.Session.Clear();

            return Ok("Usuario deslogueado exitosamente.");
        }

        /// <summary>
        /// Obtener informacion de la sesion
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("user-info")]
        public async Task<IActionResult> GetUserInfo()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                // Recuperar el objeto de la sesión
                var usuario = HttpContext.Session.GetObject<UsuarioDto>("Usuario");

                if (usuario != null)
                {
                    var result = await _usuarioService.GetUserById(usuario.CodigoUsuario);
                    return Ok(result);
                }
                return NotFound("Usuario no encontrado en la sesión.");
            }
            return Unauthorized();
        }


        /// <summary>
        /// Obtiene todos los usuarios
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
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

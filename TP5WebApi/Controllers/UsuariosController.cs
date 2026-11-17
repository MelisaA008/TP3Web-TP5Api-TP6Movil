using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TP5Shared.DTOs;
using TP5WebApi.Services;

namespace TP5WebApi.Controllers
{
    /// <summary>
    /// Gestión de autenticación y usuarios.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly AuthService _authService;

        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(
        AuthService authService,
        ILogger<UsuariosController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Registra un usuario nuevo.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult>Register(RegisterRequest request)
        {
            var resultado = await _authService.RegisterAsync(request);

            if (!resultado)
            {
                return BadRequest("El usuario ya existe");
            }

            return Ok("Usuario registrado correctamente");
        }

        /// <summary>
        /// Inicia sesión y devuelve un JWT.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult>Login(LoginRequest request)
        {
            var respuesta = await _authService.LoginAsync(request);

            if (respuesta == null)
            {
                return Unauthorized("Credenciales incorrectas");
            }

            return Ok(respuesta);
        }

        /// <summary>
        /// Devuelve todos los usuarios.
        /// Solo Admin.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<List<UsuarioDto>>>
        GetUsuarios()
        {
            try
            {
                var usuarios = await _authService.GetUsuariosAsync();

                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                _logger.LogError (ex,"Error obteniendo usuarios");

                return StatusCode(500,"No fue posible obtener usuarios.");
            }
        }

        /// <summary>
        /// Devuelve un usuario por Id.
        /// Solo Admin.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>>GetUsuario(int id)
        {
            var usuario = await _authService.GetUsuarioAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }
    }
}
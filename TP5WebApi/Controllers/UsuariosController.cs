using TP5.Shared.DTOs;
using TP5WebApi.Services;

namespace TP5WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly AuthService _authService;

        public UsuariosController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            RegisterRequest request)
        {
            var resultado =
                await _authService.RegisterAsync(request);

            if (!resultado)
                return BadRequest("El usuario ya existe");

            return Ok("Usuario registrado correctamente");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            LoginRequest request)
        {
            var respuesta =
                await _authService.LoginAsync(request);

            if (respuesta == null)
                return Unauthorized("Credenciales incorrectas");

            return Ok(respuesta);
        }
    }
}